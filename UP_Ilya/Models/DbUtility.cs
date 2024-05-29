using System.Collections.ObjectModel;
using System.Windows;
using UP_Ilya.Models;

public static class DbUtility
{
    public static void DeleteItem<T>(ObservableCollection<T> items, object itemsDataGridSelectedItem, TV_ProgramContext context) where T : class
    {
        if (items.Count > 0 && itemsDataGridSelectedItem is T selectedItem)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этот элемент?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    context.Set<T>().Remove(selectedItem);
                    context.SaveChanges();

                    items.Remove(selectedItem);
                    MessageBox.Show("Элемент успешно удален!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления элемента: {ex.Message}");
                }
            }
        }
        else
        {
            MessageBox.Show("Пожалуйста, выберите элемент для удаления.");
        }
    }
}