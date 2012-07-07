using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace WelfEditor
{
    public partial class WelfEditorForm : Form
    {
        private bool _changed;
        private List<uint> _entries;
        private Dictionary<int, ListViewItem> _cache;
        private SortOrder _sort = SortOrder.Ascending;

        public WelfEditorForm()
        {
            InitializeComponent();

            _entries = new List<uint>(2048);
            _cache = new Dictionary<int, ListViewItem>(UInt16.MaxValue);
        }

        private void OpenMenuItemClick(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            Clear();

            using (FileStream stream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                int count = reader.ReadInt32();
                for (int i = 0; i < count; ++i)
                {
                    uint entry = reader.ReadUInt32();
                    _entries.Add(entry);
                }
            }

            BuildList();
        }

        private void Clear()
        {
            _cache.Clear();
            _entries.Clear();
            idListView.Items.Clear();
            IdTextBox.Clear();
            totalIdBox.Value = 0;
            _changed = false;
        }

        private void ExitMenuItemClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void AddButtonClick(object sender, EventArgs e)
        {
            string text = IdTextBox.Text;

            uint entry;
            if (!uint.TryParse(text, out entry))
                return;

            if (_entries.Contains(entry))
                return;

            _entries.Add(entry);
            idListView.VirtualListSize = _entries.Count;

            ++totalIdBox.Value;
            _changed = true;
            removeButton.Enabled = true;
        }

        private void RemoveButtonClick(object sender, EventArgs e)
        {
            if (_entries.Count < 1)
                return;

            uint entry = 0;

            if (idListView.SelectedIndices.Count > 0)
            {
                for (int i = 0; i < idListView.SelectedIndices.Count; ++i)
                {
                    int index = idListView.SelectedIndices[i];
                    ListViewItem selectedItem = idListView.Items[index];
                    if (!uint.TryParse(selectedItem.Text, out entry))
                        return;

                    _entries.Remove(entry);
                    if (--totalIdBox.Value == 0)
                        removeButton.Enabled = false;
                }
                _cache.Clear();
                idListView.VirtualListSize = _entries.Count;

                _changed = true;
                return;
            }

            string text = IdTextBox.Text;
            if (!uint.TryParse(text, out entry))
                return;

            if (!_entries.Contains(entry))
                return;

            _cache.Clear();
            _entries.Remove(entry);
            idListView.VirtualListSize = _entries.Count;

            if (--totalIdBox.Value == 0)
                removeButton.Enabled = false;

            _changed = true;
        }

        private void NewMenuItemClick(object sender, EventArgs e)
        {
            if (!_changed)
                return;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                Save(saveFileDialog.FileName);

            Clear();
        }


        private void BuildList()
        {
            int size = _entries.Count;
            if (size < 1)
                return;

            _cache.Clear();
            idListView.Items.Clear();
            idListView.VirtualMode = true;
            idListView.VirtualListSize = size;
            idListView.EnsureVisible(size - 1);
            idListView.EnsureVisible(0);

            totalIdBox.Value = size;
            removeButton.Enabled = true;
        }

        private void Save(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(_entries.Count);
                foreach (uint entry in _entries)
                {
                    writer.Write(entry);
                }
            }
        }

        private void SaveMenuItemClick(object sender, EventArgs e)
        {
            if (_entries.Count < 1)
                return;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                Save(saveFileDialog.FileName);
        }

        private void ConvertMenuItemClick(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            Clear();

            using (StreamReader reader = new StreamReader(openFileDialog.FileName))
            {
                string text = reader.ReadToEnd();
                string[] values = text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in values)
                {
                    uint value;
                    if (!uint.TryParse(s, out value))
                        continue;

                    _entries.Add(value);
                }
            }

            saveFileDialog.FileName = openFileDialog.FileName;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                Save(saveFileDialog.FileName);
        }

        private void ListCacheVirtualItems(object sender, CacheVirtualItemsEventArgs e)
        {
            int startIndex = e.StartIndex;
            if (_cache.ContainsKey(startIndex) && _cache.ContainsKey(e.EndIndex))
                return;

            for (int i = 0; i < (e.EndIndex - startIndex + 1); ++i)
            {
                if (!_cache.ContainsKey(startIndex + i))
                    _cache.Add((startIndex + i), Item(startIndex + i));
            }
        }

        private ListViewItem Item(int index)
        {
            return new ListViewItem(_entries[index].ToString());
        }

        private void ListRetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            int index = e.ItemIndex;
            e.Item = _cache.ContainsKey(index)
                             ? _cache[index]
                             : Item(index);
        }

        private void IdListViewColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (_entries.Count < 1)
                return;

            _entries.Sort(new IdComparer(_sort));
            _sort = _sort == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;

            idListView.Items.Clear();
            _cache.Clear();

            idListView.EnsureVisible(_entries.Count - 1);
            idListView.EnsureVisible(0);
        }
    }
}
