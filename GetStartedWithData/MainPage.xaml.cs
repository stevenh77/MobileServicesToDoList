using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Runtime.Serialization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace GetStartedWithData
{    
    public class TodoItem
    {
        public int Id { get; set; }

        [DataMember(Name = "text")]
        public string Text { get; set; }

        [DataMember(Name = "complete")]
        public bool Complete { get; set; }

        //[DataMember(Name = "createdAt")]
        //public DateTime? CreatedAt { get; set; }
    }

    public sealed partial class MainPage : Page
    {
        private MobileServiceCollectionView<TodoItem> items;
        private IMobileServiceTable<TodoItem> todoTable = App.MobileService.GetTable<TodoItem>();

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void InsertTodoItem(TodoItem todoItem)
        {
            await todoTable.InsertAsync(todoItem);
        }

        private void RefreshTodoItems()
        {
            //// TODO #1: Defines a simple query for all items. 
            items = todoTable.ToCollectionView();

            //// TODO #2: More advanced query that filters out completed items. 
            //items = todoTable
            //   .Where(todoItem => todoItem.Complete == false)
            //   .ToCollectionView();
           
            ListItems.ItemsSource = items;
        }

        private async void UpdateCheckedTodoItem(TodoItem item)
        {
             await todoTable.UpdateAsync(item);     
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshTodoItems();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            var todoItem = new TodoItem { Text = TextInput.Text };
            InsertTodoItem(todoItem);
        }

        private void CheckBoxComplete_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            TodoItem item = cb.DataContext as TodoItem;
            UpdateCheckedTodoItem(item);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            RefreshTodoItems();
        }
    }
}