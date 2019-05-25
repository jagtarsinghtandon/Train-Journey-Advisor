using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Xml;
using Train.Classes;

namespace Train
{

    public partial class MainWindow : Window
    {

        public string Source { get; set; }
        public string Destination { get; set; }
        public string path { get; set; }
        public int ForDiscount { get; set; }
        public double AdultDiscount { get; set; }
        public double ChildDiscount { get; set; }
        ObservableCollection<Routes> routes;
        ObservableCollection<Tickets> tickets;

        public MainWindow()
        {
            InitializeComponent();
            tickets = new ObservableCollection<Tickets>();
            routes = MyStorage.ReadXML<ObservableCollection<Routes>>("NewRoutes.xml");           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Train_Image.Visibility = Visibility.Visible;
            HidePath();

            tickets = MyStorage.ReadXML<ObservableCollection<Tickets>>("TicketsNew.xml");

            List<string> cities = Cities("Cities.xml", "Source");
            foreach (string s in cities)
            {
                Cbx_Source.Items.Add(s);
                Cbx_Destination.Items.Add(s);

            }
            List<string> concession = Cities("Cities.xml", "Concession");
            foreach (string s in concession)
            {
                Cbx_Concession.Items.Add(s);

            }
            List<string> tickettype = Cities("Cities.xml", "TicketType");
            foreach (string s in tickettype)
            {
                Cbx_Ticket_Type.Items.Add(s);
            }
        }

        private void HidePath()
        {
            Delhi_Bhopal.Visibility = Visibility.Collapsed;
            Delhi_Jaipur.Visibility = Visibility.Collapsed;
            Jaipur_Mumbai.Visibility = Visibility.Collapsed;
            Bhopal_Hyderabad.Visibility = Visibility.Collapsed;
            Hyderabad_Bangalore.Visibility = Visibility.Collapsed;
            Mumbai_Bangalore.Visibility = Visibility.Collapsed;
            Bhopal_Mumbai.Visibility = Visibility.Collapsed;
            Jaipur_Bhopal.Visibility = Visibility.Collapsed;
            Jaipur_Hyderabad.Visibility = Visibility.Collapsed;
            Jaipur_Bangalore.Visibility = Visibility.Collapsed;
            Mumbai_Hyderabad.Visibility = Visibility.Collapsed;
            Bhopal_Bangalore.Visibility = Visibility.Collapsed;
            Mumbai_Delhi.Visibility = Visibility.Collapsed;
            Delhi_Bangalore.Visibility = Visibility.Collapsed;
            Delhi_Hyderabad.Visibility = Visibility.Collapsed;
        }

        private List<string> Cities(string fileName, string tagName)
        {
            List<string> Items = new List<string>();
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);

            XmlNodeList nodeItems = doc.GetElementsByTagName(tagName);
            foreach (XmlNode nodeItem in nodeItems)
            {
                Items.Add(nodeItem.InnerText);
            }

            return Items;
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        public void Btn_Search(object sender, RoutedEventArgs e)
        {
            if (Source == Destination)
            {
                MessageBox.Show("Enter correct Source & Destination", "Alert");
            }
            else if (Source == null)
            {
                MessageBox.Show("Please enter Source!", "Alert");
            }
            else if (Destination == null)
            {
                MessageBox.Show("Please enter Destination!", "Alert");
            }

            HidePath();

            List<Routes> route = new List<Routes>();
            foreach (var item in routes)
            {
                string[] routeArr = item.route.Split('-');
                if (Array.IndexOf(routeArr, Source) < Array.IndexOf(routeArr, Destination) && Array.IndexOf(routeArr, Source) != -1 && (Array.IndexOf(routeArr, Destination) != -1))
                    route.Add(item);
            }
            Lbx_Routes.ItemsSource = route;
        }

        private void Cbx_Source_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Source = Cbx_Source.SelectedValue.ToString();

        }

        private void Cbx_Destination_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Destination = Cbx_Destination.SelectedValue.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((Tbx_Name.Text != null) && (Tbx_Age.Text != null) && (Date.SelectedDate != null) &&
                  (Cbx_Concession.SelectedValue != null)
                  && (Cbx_Ticket_Type.SelectedValue != null) && (Tbx_Total.Text != null))
            {
                var t = new Tickets
                {
                    Name = Tbx_Name.Text,
                    Age = int.Parse(Tbx_Age.Text),
                    Concession = Cbx_Concession.SelectedItem.ToString(),
                    TicketType = Cbx_Ticket_Type.SelectedItem.ToString(),
                    Total = Tbx_Total.Text.ToString(),
                    Path = theRoute,
                    Date = (DateTime)Date.SelectedDate
                };

                tickets.Add(t);

                MyStorage.SaveXML<ObservableCollection<Tickets>>(tickets, "TicketsNew.xml");
                MessageBox.Show("Ticket Booked successfully");
            }
            else
            {
                MessageBox.Show("Please enter all the details first!", "Alert");
            }
        }


        private void Elp_Delhi_MouseUp(object sender, MouseButtonEventArgs e)
        {
            AddCity(sender);

        }

        private void Elp_Bhopal_MouseUp(object sender, MouseButtonEventArgs e)
        {
            AddCity(sender);
        }

        private void Elp_Hyderabad_MouseUp(object sender, MouseButtonEventArgs e)
        {
            AddCity(sender);

        }
        private void AddCity(object sender)
        {

            HidePath();
            String city = (sender as Ellipse).ToolTip.ToString();

            int count = (from r in theRoute where r.Equals(city) select r).ToList().Count;
            if (count == 0 && Lbx_Route.SelectedIndex != -1)
            {
                theRoute.Insert(Lbx_Route.SelectedIndex, city);
                DisplayRoute(theRoute);
            }
            else
            {
                if (Lbx_Route.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a city in the Selected Route List " +
                        "before which you would like to add your new city!!", "Important");
                }
                else
                {
                    MessageBox.Show("City already exists in the route");
                }

            }
        }

        private void Elp_Bangalore_MouseUp(object sender, MouseButtonEventArgs e)
        {
            AddCity(sender);
        }

        private void Elp_Mumbai_MouseUp(object sender, MouseButtonEventArgs e)
        {
            AddCity(sender);
        }

        private void Elp_Jaipur_MouseUp(object sender, MouseButtonEventArgs e)
        {
            AddCity(sender);
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            if ((Tbx_Name.Text != null) && (Tbx_Age.Text != null) && (Date.SelectedDate != null) &&
                (Cbx_Concession.SelectedValue != null)
                && (Cbx_Ticket_Type.SelectedValue != null) && (Tbx_Total.Text != null))
            {
                var win = new PrintTicket();
                win.Show();
                win.Owner = this;
                this.Visibility = Visibility.Collapsed;

                Tickets tickets = new Tickets();

                win.Txt_Receipt.AppendText("=====================================================" + "\n");
                win.Txt_Receipt.AppendText("Name: " + Tbx_Name.Text);
                win.Txt_Receipt.AppendText(" | " + "Age:" + Tbx_Age.Text + "\n");
                win.Txt_Receipt.AppendText("=====================================================" + "\n");
                win.Txt_Receipt.AppendText("Route: " + PrintRoute() + "\n");
                win.Txt_Receipt.AppendText("=====================================================" + "\n");
                win.Txt_Receipt.AppendText("Date: " + Date.SelectedDate + "\n");
                win.Txt_Receipt.AppendText("=====================================================" + "\n");
                win.Txt_Receipt.AppendText("Concession: " + Cbx_Concession.SelectedValue.ToString());
                win.Txt_Receipt.AppendText("\n" + "Ticket type: " + Cbx_Ticket_Type.SelectedValue.ToString());
                win.Txt_Receipt.AppendText("\n=====================================================" + "\n");
                win.Txt_Receipt.AppendText("Total: " + Tbx_Total.Text);
            }
            else
                MessageBox.Show("Please enter all the details first & Press Book Ticket!", "Alert");

        }

        private void Btn_Route1_Click(object sender, RoutedEventArgs e)
        {
            Train_Image.Visibility = Visibility.Visible;
        }

        ObservableCollection<string> theRoute;
        private void Lbx_Routes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Tbx_Final_Route.Visibility = Visibility.Visible;
            Lbx_Route.Visibility = Visibility.Visible;

            if (Lbx_Routes.SelectedItem != null)
            {
                HidePath();
                theRoute = new ObservableCollection<string>();
                var test = (Lbx_Routes.SelectedItem as Routes).route.Split('-');
                for (int i = 0; i < test.Length; i++)
                {
                    theRoute.Add(test[i]);
                }
                Lbx_Route.ItemsSource = theRoute;
                DisplayRoute(theRoute);
            }
        }

        public void DisplayRoute(ObservableCollection<string> theRoute)
        {

            int totalsum = 0;
            for (int i = 0; i < theRoute.Count; i++)
            {
                if (i + 1 == theRoute.Count)
                    break;
                else
                {
                    var line = FindName(theRoute[i] + "_" + theRoute[i + 1]) as Line;
                    if (line == null)

                    {
                        var newLine = FindName(theRoute[i + 1] + "_" + theRoute[i]) as Line;
                        newLine.Visibility = Visibility.Visible;
                        totalsum = totalsum + int.Parse((newLine as Line).ToolTip.ToString());
                        Tbx_Total.Text = "₹ " + totalsum.ToString();
                        ForDiscount = totalsum;
                    }

                    else
                    {
                        line.Visibility = Visibility.Visible;
                        totalsum = totalsum + int.Parse((line as Line).ToolTip.ToString());
                        Tbx_Total.Text = "₹ " + totalsum.ToString();
                        ForDiscount = totalsum;
                    }
                }

            }

        }

        private void Lbx_Route_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayRoute(theRoute);
        }

        private string PrintRoute()
        {
            string printpath = "";

            foreach (var item in theRoute)
            {
                printpath += item + " ";
            }
            return printpath;

        }

        public static bool IsValid(string str)
        {
            int i;
            return int.TryParse(str, out i) && i <= 999;
        }

        private void Tbx_Total_PreviewTextInput(object sender, TextCompositionEventArgs e)

        {
            e.Handled = !CheckValid(((TextBox)sender).Text + e.Text);
        }

        public static bool CheckValid(string str)
        {
            int i;
            return int.TryParse(str, out i);

        }

        public class AlphabetTextBox : TextBox
        {
            private static readonly Regex regex = new Regex("^[a-zA-Z]+$");

            protected override void OnPreviewTextInput(TextCompositionEventArgs e)
            {
                if (!regex.IsMatch(e.Text))
                    e.Handled = true;
                base.OnPreviewTextInput(e);
            }
        }

        public void Cbx_Concession_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            double Discount;
            if (Cbx_Concession.SelectedValue.ToString() == "Senior Citizen (20% Discount)")
            {
                Discount = (0.8) * ForDiscount;
                AdultDiscount = Discount;

                Tbx_Total.Text = "₹ " + Discount.ToString();
            }
            else if (Cbx_Concession.SelectedValue.ToString() == "Child under 6 years (50% Discount)")
            {
                Discount = (0.5) * ForDiscount;

                ChildDiscount = Discount;
                Tbx_Total.Text = "₹ " + Discount.ToString();
            }
            else if (Cbx_Concession.SelectedValue.ToString() == "No Concession")
            {
                Discount = ForDiscount;
                Tbx_Total.Text = "₹ " + Discount.ToString();
            }
        }

        public void Cbx_Ticket_Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            double Discount2;

            if ((Cbx_Ticket_Type.SelectedValue.ToString() == "Return Journey") &&
               (Cbx_Concession.SelectedValue.ToString() == "Child under 6 years (50% Discount)"))
            {
                Discount2 = 2 * ChildDiscount;
                Tbx_Total.Text = "₹ " + Discount2.ToString();
            }

            else if ((Cbx_Ticket_Type.SelectedValue.ToString() == "Return Journey") &&
               (Cbx_Concession.SelectedValue.ToString() == "Senior Citizen (20% Discount)"))
            {
                Discount2 = 2 * AdultDiscount;
                Tbx_Total.Text = "₹ " + Discount2.ToString();
            }

            if ((Cbx_Ticket_Type.SelectedValue.ToString() == "Single Journey") &&
               (Cbx_Concession.SelectedValue.ToString() == "Child under 6 years (50% Discount)"))
            {
                Discount2 = ChildDiscount;
                Tbx_Total.Text = "₹ " + Discount2.ToString();
            }

            if ((Cbx_Ticket_Type.SelectedValue.ToString() == "Single Journey") &&
                (Cbx_Concession.SelectedValue.ToString() == "Senior Citizen (20% Discount)"))
            {
                Discount2 = AdultDiscount;
                Tbx_Total.Text = "₹ " + Discount2.ToString();
            }
            if ((Cbx_Ticket_Type.SelectedValue.ToString() == "Return Journey") &&
                (Cbx_Concession.SelectedValue.ToString() == "No Concession"))
            {
                Discount2 = 2 * ForDiscount;
                Tbx_Total.Text = "₹ " + Discount2.ToString();
            }

        }

    }
}

