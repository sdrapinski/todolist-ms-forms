using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.IO;

namespace todolist
{
    public partial class Form1 : Form
    {
        private BindingList<ToDoEntry> entries = new BindingList<ToDoEntry>();
        public Form1()
        {
            InitializeComponent();
            titleInput.DataBindings.Add("Text", entriesSource,"Title" ,true, DataSourceUpdateMode.OnPropertyChanged);
            descriptionBox.DataBindings.Add("Text", entriesSource, "Description", true, DataSourceUpdateMode.OnPropertyChanged);
            dateTimePicker1.DataBindings.Add("Value", entriesSource, "DueDate", true, DataSourceUpdateMode.OnPropertyChanged);
           
            entriesSource.DataSource = entries;
            createNewItem();
        }

         private void createNewItem()
        {
            ToDoEntry newEntry = (ToDoEntry)entriesSource.AddNew();
            newEntry.Title = "nowe zadanie";
            newEntry.Description = "nowy opis";
            newEntry.DueDate = DateTime.Now;
            entriesSource.ResetCurrentItem();
        }


        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void entriesSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            switch(e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    MakeListViewItemForNewEntry(e.NewIndex);
                    break;
                case ListChangedType.ItemDeleted:
                    RemoveListViewItem(e.NewIndex);
                    break;
                case ListChangedType.ItemChanged:
                    UpdateListViewItem(e.NewIndex);
                    break;
            }
        }

        private void MakeListViewItemForNewEntry(int newItemIndex)
        {
            ListViewItem item = new ListViewItem();
            item.SubItems.Add("");
            listView1.Items.Insert(newItemIndex,item);
        }

        private void RemoveListViewItem(int deletedItemIndex)
        {
            listView1.Items.RemoveAt(deletedItemIndex);
        }

        private void UpdateListViewItem(int itemIndex)
        {
            ListViewItem listItem = listView1.Items[itemIndex];
            ToDoEntry entry = entries[itemIndex];
            listItem.SubItems[0].Text = entry.Title;
            listItem.SubItems[1].Text = entry.DueDate.ToString();
        }

        private void deleteButton_MouseClick(object sender, MouseEventArgs e)
        {
            if(listView1.SelectedItems.Count >0)
            {
                int index = listView1.SelectedIndices[0];
                entriesSource.RemoveAt(index);
            }
         
        }

        private void newButton_MouseClick(object sender, MouseEventArgs e)
        {
            createNewItem();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                int index = listView1.SelectedIndices[0];
                entriesSource.Position = index;
            }
        }

        private void buttonToFile_Click(object sender, EventArgs e)
        {
            // export
            StreamWriter sw = new StreamWriter("todo.json");
            string jsonString = JsonSerializer.Serialize(entries);
            sw.WriteLine(jsonString);
            sw.Close();

        }

        private void buttonfromfile_Click(object sender, EventArgs e)
        {
            //import
            StreamReader sr = new StreamReader("todo.json");
           string line = sr.ReadLine();
           entries = JsonSerializer.Deserialize<BindingList<ToDoEntry>>(line);
            sr.Close();

        }
    }
}
