using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SitdikovAA41
{

    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        int CountRecords;//Количество записей в таблице
        int CountPage;//Общее количество страниц
        int CurrentPage = 0;//Текущая страница

        List<Product> CurrentPageList = new List<Product>();
        List<Product> TableList;

        private User _user;
        public ProductPage(User user)
        {
            string logi; string rol;
            InitializeComponent(); _user = user;
            var currentProducts = Sitdi41Entities.GetContext().Product.ToList(); ProductListView.ItemsSource = currentProducts;
            if (user != null)
            {
                logi = user.UserSurname + user.UserName  + " " + user.UserPatronymic; 
                Imya.Text = logi;
                switch (user.UserRole)
                {
                    case 1:
                        rol = "Клиент";
                        break;
                    case 2:
                        rol = "Менеджер"; break;
                    case 3:
                        rol = "Администратор";
                        break;
                    default:
                        rol = "Гость"; break;
                }
            }
            else
            {
                rol = "Гость";
            }
            Role.Text = rol;
            UpdateProduct();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage());
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProduct();

        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProduct();

        }

        private void RButtonUp_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProduct();

        }

        private void RButtonDown_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProduct();

        }

        //private void ChangePage(int direction, int? selectedPage)
        //{
        //    CurrentPageList.Clear();
        //    CountRecords = TableList.Count;

        //    if (CountRecords % 10 > 0)
        //    {
        //        CountPage = CountRecords / 10 + 1;
        //    }
        //    else
        //    {
        //        CountPage = CountRecords / 10;
        //    }

        //    Boolean Ifupdate = true;

        //    int min;

        //    if (selectedPage.HasValue)
        //    {
        //        if (selectedPage >= 0 && selectedPage <= CountPage)
        //        {
        //            CurrentPage = (int)selectedPage;
        //            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
        //            for (int i = CurrentPage * 10; i < min; i++)
        //            {
        //                CurrentPageList.Add(TableList[i]);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        switch (direction)
        //        {
        //            case 1:
        //                if (CurrentPage > 0)
        //                {
        //                    CurrentPage--;
        //                    min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
        //                    for (int i = CurrentPage * 10; i < min; i++)
        //                    {
        //                        CurrentPageList.Add(TableList[i]);
        //                    }
        //                }
        //                else
        //                {
        //                    Ifupdate = false;
        //                }
        //                break;

        //            case 2:
        //                if (CurrentPage < CountPage - 1)
        //                {
        //                    CurrentPage++;
        //                    min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
        //                    for (int i = CurrentPage * 10; i < min; i++)
        //                    {
        //                        CurrentPageList.Add(TableList[i]);
        //                    }
        //                }
        //                else
        //                {
        //                    Ifupdate = false;
        //                }
        //                break;
        //        }
        //    }
        //    if (Ifupdate)
        //    {
        //        PageListBox.Items.Clear();

        //        for (int i = 1; i <= CountPage; i++)
        //        {
        //            PageListBox.Items.Add(i);
        //        }
        //        PageListBox.SelectedIndex = CurrentPage;

        //        min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;

        //        //Это вывод надписи 10 из 100 и тд
        //        TBCount.Text = min.ToString();
        //        TBAllRecords.Text = " из " + CountRecords.ToString();

        //        ProductListView.ItemsSource = CurrentPageList;
        //        ProductListView.Items.Refresh();
        //    }
        //}

        private void UpdateProduct()
        {
            var currentServices = Sitdi41Entities.GetContext().Product.ToList();

            if (ComboType.SelectedIndex == 0)
            {
                currentServices = currentServices.Where(p => (Convert.ToInt32(p.ProductDiscountAmount) >= 0 && Convert.ToInt32(p.ProductDiscountAmount) <= 100)).ToList();
            }

            if (ComboType.SelectedIndex == 1)
            {
                currentServices = currentServices.Where(p => (Convert.ToInt32(p.ProductDiscountAmount) >= 0 && Convert.ToInt32(p.ProductDiscountAmount) < 9.99)).ToList();
            }
            if (ComboType.SelectedIndex == 2)
            {
                currentServices = currentServices.Where(p => (Convert.ToInt32(p.ProductDiscountAmount) >= 10 && Convert.ToInt32(p.ProductDiscountAmount) < 14.99)).ToList();
            }
            if (ComboType.SelectedIndex == 3)
            {
                currentServices = currentServices.Where(p => (Convert.ToInt32(p.ProductDiscountAmount) >= 15 && Convert.ToInt32(p.ProductDiscountAmount) < 100)).ToList();
            }


            currentServices = currentServices.Where(p => p.ProductName.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();
            ProductListView.ItemsSource = currentServices.ToList();

            if (RButtonDown.IsChecked.Value)
            {
                currentServices = currentServices.OrderByDescending(p => p.ProductCost).ToList();
            }
            if (RButtonUp.IsChecked.Value)
            {
                currentServices = currentServices.OrderBy(p => p.ProductCost).ToList();

            }
            ProductListView.ItemsSource = currentServices;
            TableList = currentServices;
            счёт.Text = "Количество " + currentServices.Count.ToString() + " из " + Sitdi41Entities.GetContext().Product.ToList().Count.ToString();
        }

    


        private void ComboType_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            UpdateProduct();

        }


        //private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    ChangePage(0, Convert.ToInt32(PageListBox.SelectedItem.ToString()) - 1);

        //}

    }
}


