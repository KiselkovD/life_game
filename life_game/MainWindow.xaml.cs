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
using System.Windows.Threading;
using System.IO;

namespace coursework_wpf
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string[] color_ = new string[6] { "#FFDC143C", "#FF560074","#FFADD8E6","#FF008000","#FFA52A2A","#FF36393F" };
        public int[] array = new int[1] { 1 };
        public int cell_type = 0;
        public int input_type = 0;
        public double speed = 0;
        public bool fl_pause =true,fl_menu = true;
        public int buff = 666;
        public int buff2 = 0;
        public int sav_buf = 1;
        public int lod_buf = 1;
        DispatcherTimer gameloop;
        Net net = new Net();
        Net net_buf = new Net();
        public MainWindow()
        {
            InitializeComponent();
            int tt = 1;
            foreach (UIElement el in gameField.Children)//создаем события для работы с сеткой
            {

                if (el is Button)
                {
                    ((Button)el).Click += Button_Click;
                    ((Button)el).FontSize = tt;//"a" + tt.ToString();
                    tt++;
                }
            }
            for (int dd = 1; dd < 14; dd++)//создаем события для работы с сеткой
            {
                if (load.Children[dd] is Button)
                {
                    ((Button)load.Children[dd]).Click += Button_loads_Click;
}
                if (save.Children[dd] is Button)
                { 
                    ((Button)save.Children[dd]).Click += Button_saves_Click;
                }
            }
            //((Button)gameField.Children[20]).Content = ((Button)gameField.Children[20]).FontSize;

            //net.Cells[2].activate(2, ref net.Cells);
            Console.WriteLine() ;
            InitializeTimer();

            //////////////////////////

            //while (fl_pause==false)


            //////////////////////////
            /*
             todo
            дописать функции классов
            
            организовать сохранение загрузку
            организовать смену страниц
            организовать переменные для хранения параметров введенных пользователем
            /наверное просто делать обнуление цвета всей таблицы при нажатии
            замена новая игра/продолжить
            основной цикл:
                #считываем действия игрока
                #поле с расклассоваными клеками
                #проходимся по полю вызывая его обитателей
                #проходимся по полю заполняя клетки
            /наверно сделать защитную пластину против нажатий не помешает
             */
        }
        private void InitializeTimer()
        {
            var interval = TimeSpan.FromSeconds(0.1+speed).Ticks;
            gameloop = new DispatcherTimer
            {
                Interval = TimeSpan.FromTicks(interval)
            };
            gameloop.Tick += MainGameLoop;
            gameloop.Start();
        }
        private void MainGameLoop(object sender, EventArgs e)////////////////////////////////////////////*******************************************
        {
            if(speed != 0)
            {
                 for (int ii = 0; ii < Const.MAX_GRID * Const.MAX_GRID; ii++) net.Cells[ii].activate(ii, ref net.Cells);
                 for (int ii = 0; ii < Const.MAX_GRID * Const.MAX_GRID; ii++) net.Cells[ii].spawn(ii, ref net.Cells);
            }
            for (int ii = 0; ii < Const.MAX_GRID * Const.MAX_GRID; ii++) ((Button)gameField.Children[ii]).Background = (Brush)converter.ConvertFromString(color_[net.Cells[ii].who()]);

        }
        //coursework_wpf.MainWindow
        BrushConverter converter = new System.Windows.Media.BrushConverter();
        private void Button_saves_Click(object sender, RoutedEventArgs e)//выводит из файла
        {
            using (StreamReader sr = new StreamReader(File.Open($"{((Button)e.OriginalSource).Content}.txt", FileMode.OpenOrCreate)))
            {
                for (int lol = 0; lol < 144; lol++)
                {
                    switch (Convert.ToInt32(sr.ReadLine()))//убийцы0 обычные1 социальные2 живучие3 еда4
                    {
                        case 0:
                            net_buf.Cells[lol] = new killer();
                            break;
                        case 1:
                            net_buf.Cells[lol] = new usual();
                            break;
                        case 2:
                            net_buf.Cells[lol] = new social();
                            break;
                        case 3:
                            net_buf.Cells[lol] = new helther();
                            break;
                        case 4:
                            net_buf.Cells[lol] = new food();
                            break;
                        case 5:
                            net_buf.Cells[lol] = new Cell();
                            break;
                    }
                }

            }//читаем во 2ю
            for (int ii = 0; ii < Const.MAX_GRID * Const.MAX_GRID; ii++) ((Button)saveField.Children[ii]).Background = (Brush)converter.ConvertFromString(color_[net_buf.Cells[ii].who()]);
            sav_buf = Convert.ToInt32(((Button)e.OriginalSource).Content);
            //рисуем
        }
        private void Button_save_this_Click(object sender, RoutedEventArgs e)//игровое поле в файл
        {
            using (StreamWriter sr = new StreamWriter(File.Open($"{sav_buf}.txt", FileMode.Open)))
            {
                for (int lol = 0; lol < 144; lol++)
                {
                    sr.WriteLine(net.Cells[lol].who());

                }
            }
            

        }
        private void Button_loads_Click(object sender, RoutedEventArgs e)
        {
            using (StreamReader sr = new StreamReader(File.Open($"{((Button)e.OriginalSource).Content}.txt", FileMode.Open)))
            {
                for (int lol = 0; lol < 144; lol++)
                {
                    switch (Convert.ToInt32(sr.ReadLine()))//убийцы0 обычные1 социальные2 живучие3 еда4
                    {
                        case 0:
                            net_buf.Cells[lol] = new killer();
                            break;
                        case 1:
                            net_buf.Cells[lol] = new usual();
                            break;
                        case 2:
                            net_buf.Cells[lol] = new social();
                            break;
                        case 3:
                            net_buf.Cells[lol] = new helther();
                            break;
                        case 4:
                            net_buf.Cells[lol] = new food();
                            break;
                        case 5:
                            net_buf.Cells[lol] = new Cell();
                            break;
                    }
                }

            }
            //читаем во 2ю
            for (int ii = 0; ii < Const.MAX_GRID * Const.MAX_GRID; ii++) ((Button)loadField.Children[ii]).Background = (Brush)converter.ConvertFromString(color_[net_buf.Cells[ii].who()]);

            //рисуем
            lod_buf = Convert.ToInt32(((Button)e.OriginalSource).Content);
        }
        private void Button_load_this_Click(object sender, RoutedEventArgs e)//перекидывает игровое поле из файла
        {
            for (int lol = 0; lol < 144; lol++)
            {
                switch (net_buf.Cells[lol].who())//убийцы0 обычные1 социальные2 живучие3 еда4
                {
                    case 0:
                        net.Cells[lol] = new killer();
                        break;
                    case 1:
                        net.Cells[lol] = new usual();
                        break;
                    case 2:
                        net.Cells[lol] = new social();
                        break;
                    case 3:
                        net.Cells[lol] = new helther();
                        break;
                    case 4:
                        net.Cells[lol] = new food();
                        break;
                    case 5:
                        net.Cells[lol] = new Cell();
                        break;
                }
            }
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch (input_type)
            {
                case 0:
                    switch (cell_type)//убийцы0 обычные1 социальные2 живучие3 еда4
                    {
                        case 0:
                            ((Button)e.OriginalSource).Background = (Brush)converter.ConvertFromString("#FFDC143C");
                            net.Cells[Convert.ToInt32(((Button)e.OriginalSource).FontSize) - 1] = new killer();
                            break;
                        case 1:
                            ((Button)e.OriginalSource).Background = (Brush)converter.ConvertFromString("#FF560074"); 
                            net.Cells[Convert.ToInt32(((Button)e.OriginalSource).FontSize) - 1] = new usual();
                            break;
                        case 2:
                            ((Button)e.OriginalSource).Background = (Brush)converter.ConvertFromString("#FFADD8E6");
                            net.Cells[Convert.ToInt32(((Button)e.OriginalSource).FontSize) - 1] = new social();
                            break;
                        case 3:
                            ((Button)e.OriginalSource).Background = (Brush)converter.ConvertFromString("#FF008000");
                            net.Cells[Convert.ToInt32(((Button)e.OriginalSource).FontSize) - 1] = new helther();
                            break;
                        case 4:
                            ((Button)e.OriginalSource).Background = (Brush)converter.ConvertFromString("#FFA52A2A");
                            net.Cells[Convert.ToInt32(((Button)e.OriginalSource).FontSize) - 1] = new food();
                            break;
                        case 5:
                            ((Button)e.OriginalSource).Background = (Brush)converter.ConvertFromString("#FF36393F");
                            net.Cells[Convert.ToInt32(((Button)e.OriginalSource).FontSize) - 1] = new Cell();
                            break;
                    }
                    break;
                case 1:
                    if (buff == 666)buff = Convert.ToInt32(((Button)e.OriginalSource).FontSize)-1; 
                    else
                    {
                        buff2 = Convert.ToInt32(((Button)e.OriginalSource).FontSize) - 1;
                        int xxma;
                        int xxmi;
                        int yyma;
                        int yymi;
                        int xx = buff % Const.MAX_GRID;
                        int yy = (buff - xx) / Const.MAX_GRID;
                        int xx2 = buff2 % Const.MAX_GRID;
                        int yy2 = (buff2 - xx2) / Const.MAX_GRID;
                        if (xx > xx2)
                        {
                             xxma = xx;
                             xxmi = xx2;
                        }
                        else
                        {
                             xxmi = xx;
                             xxma = xx2;
                        }
                        if (yy > yy2)
                        {
                             yyma = yy;
                             yymi = yy2;
                        }
                        else
                        {
                             yymi = yy;
                             yyma = yy2;
                        }
                        /*using (StreamWriter sw = new StreamWriter(File.Open("log.txt", FileMode.Open)))
                        {
                            sw.WriteLine(xxmi.ToString());
                            sw.WriteLine(xxma.ToString());
                            sw.WriteLine(yymi.ToString());
                            sw.WriteLine(yyma.ToString());
                            sw.WriteLine("  ");
                            //buff.ToString() = sr.ReadLine();
                        }*/
                        for (int xg = xxmi; xg < xxma+1; xg++)
                        {
                            for (int yg = yymi; yg < yyma+1; yg++)
                            {
                                //((Button)gameField.Children[xg+yg* Const.MAX_GRID]).Background = (Brush)converter.ConvertFromString(color_[cell_type]);
                                switch (cell_type)//убийцы0 обычные1 социальные2 живучие3 еда4
                                {
                                    case 0:
                                        net.Cells[xg + yg * Const.MAX_GRID] = new killer();
                                        break;
                                    case 1:
                                        net.Cells[xg + yg * Const.MAX_GRID] = new usual();
                                        break;
                                    case 2:
                                        net.Cells[xg + yg * Const.MAX_GRID] = new social();
                                        break;
                                    case 3:
                                        net.Cells[xg + yg * Const.MAX_GRID] = new helther();
                                        break;
                                    case 4:
                                        net.Cells[xg + yg * Const.MAX_GRID] = new food();
                                        break;
                                    case 5:
                                        net.Cells[xg + yg * Const.MAX_GRID] = new Cell();
                                        break;
                                }
                                 
                            }
                        }
                        buff = 666;
                    }
                    break; 
                case 2:
                    Console.WriteLine(" s = 2"); // Выполнится, если s равно 2
                    break;
                    //((Button)e.OriginalSource).Background = (Brush)converter.ConvertFromString("#FF560074");
                    //yyyyy.Content = ((Button)e.OriginalSource).Grid.Row
            }
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                speed = 0;
                fl_menu = true;
                menuWin.Visibility = Visibility.Visible;
                saveWin.Visibility = Visibility.Collapsed;
                loadWin.Visibility = Visibility.Collapsed;
                gameWin.Visibility = Visibility.Collapsed;
                ruleWin.Visibility = Visibility.Collapsed;
            }

        }
        private void mouse_Click(object sender, MouseEventArgs e)
        {

            //string x = (string)((Button)e.OriginalSource).Content;//получить содержимое   
            //Tabl.Children[0].


            Rectangle el = new Rectangle();
            el.Width = 50;
            el.Height = 50;
            el.VerticalAlignment = VerticalAlignment.Top;
            el.Fill = Brushes.Green;
            el.Stroke = Brushes.Red;
            el.StrokeThickness = 3;
            //for_game.Children.Add(el);

           // = "X=" + e.GetPosition(null).X + " Y=" + e.GetPosition(null).Y;

            ((Button)e.OriginalSource).Background = (Brush)converter.ConvertFromString("#FF560074");
            Background = (Brush)converter.ConvertFromString("#FF560074");
        }


        //public Class1 dd;убийцы0 обычные1 социальные2 живучие3 еда4
        private void Button_u(object sender, RoutedEventArgs e)
        {
            cell_type = 1;
        }
        private void Button_s(object sender, RoutedEventArgs e)
        {
            cell_type = 2;
        }
        private void Button_h(object sender, RoutedEventArgs e)
        {
            cell_type = 3;
        }
        private void Button_k(object sender, RoutedEventArgs e)
        {
            cell_type = 0;
        }
        private void Button_f(object sender, RoutedEventArgs e)
        {
            cell_type = 4;
        }
        private void Button_c(object sender, RoutedEventArgs e)
        {
            cell_type = 5;
        }
        private void one(object sender, RoutedEventArgs e)
        {
            input_type = 0;
        }
        private void many(object sender, RoutedEventArgs e)
        {
            input_type = 1;
        }
        private void rand(object sender, RoutedEventArgs e)
        {
            input_type = 2;
        }
        
        private void speed0(object sender, RoutedEventArgs e)
        {
            speed = 0;
        }
        private void speed1(object sender, RoutedEventArgs e)
        {
            speed = 1;
        }
        private void speed2(object sender, RoutedEventArgs e)
        {
            speed = 0.5;
        }

        private void menu_Click_1(object sender, RoutedEventArgs e)
        {
            fl_menu = false;
            menuWin.Visibility = Visibility.Collapsed;
            saveWin.Visibility = Visibility.Collapsed;
            loadWin.Visibility = Visibility.Collapsed;
            gameWin.Visibility = Visibility.Visible;
            ruleWin.Visibility = Visibility.Collapsed;
            //новая игра
        }

        private void menu_Click_2(object sender, RoutedEventArgs e)
        {
            fl_menu = false;
            menuWin.Visibility = Visibility.Collapsed;
            saveWin.Visibility = Visibility.Visible;
            loadWin.Visibility = Visibility.Collapsed;
            gameWin.Visibility = Visibility.Collapsed;
            ruleWin.Visibility = Visibility.Collapsed;
            //сохранить
        }

        private void menu_Click_3(object sender, RoutedEventArgs e)
        {
            fl_menu = false;
            menuWin.Visibility = Visibility.Collapsed;
            saveWin.Visibility = Visibility.Collapsed;
            loadWin.Visibility = Visibility.Visible;
            gameWin.Visibility = Visibility.Collapsed;
            ruleWin.Visibility = Visibility.Collapsed;
            //загрузить
        }

        private void menu_Click_4(object sender, RoutedEventArgs e)
        {
            fl_menu = false;
            menuWin.Visibility = Visibility.Collapsed;
            saveWin.Visibility = Visibility.Collapsed;
            loadWin.Visibility = Visibility.Collapsed;
            gameWin.Visibility = Visibility.Collapsed;
            ruleWin.Visibility = Visibility.Visible;
            //правила
        }

        private void menu_Click_5(object sender, RoutedEventArgs e)
        {
            //выход
            Application.Current.Shutdown();
        }

        //private void TextBox_KeyDown(object sender, KeyEventArgs e)
        //{
            /*if (e.Key == Key.Escape) //OemQuotes
                Application.Current.Shutdown(); // добавляем кавычки
            else
                menu.Content += e.Key.ToString();*/
        //}
        

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int d = ((MainWindow)Application.Current.MainWindow).array[0];

            MessageBox.Show(d.ToString());
        }
    }
}

/*
 private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).array[0] = 0;
            //((MainWindow)Application.Current.MainWindow).menu.Content = "Some text";
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Hide();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int d = ((MainWindow)Application.Current.MainWindow).array[0];

            MessageBox.Show(d.ToString());
        }
 */


/*
 foreach (UIElement el in Tabl.Children)
            {
                if (el is Button)
                {
                    ((Button)el).Click += Button_Click;
                }
            }
        }
        //coursework_wpf.MainWindow
        BrushConverter converter = new System.Windows.Media.BrushConverter();
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //string x = (string)((Button)e.OriginalSource).Content;//получить содержимое   
            //Tabl.Children[0].

            ((Button)e.OriginalSource).Background = (Brush)converter.ConvertFromString("#FF560074");
        }
 */



/*
 void LayoutRoot_MouseMove(object sender, MouseEventArgs e)
        {
            // = "X=" + e.GetPosition(null).X + " Y=" + e.GetPosition(null).Y;
        }
 */


/*
 <Window.Resources><Style <Setter<Setter.Value><ControlTemplate <ControlTemplate.Triggers> <Trigger<Setter
        <ImageBrush x:Key="google_icon" ImageSource="C:\Users\denis\source\repos\coursework_wpf\Assets/g.png" Stretch="None"/>
        <Style x:Key="googleStyle" TargetType="{x:Type Button}">
            <Setter Property="Template">
                <Setter.Value>
                    <ControlTemplate TargetType="{x:Type Button}">
                        <Border x:Name="border" BorderBrush="{TemplateBinding BorderBrush}" BorderThickness="{TemplateBinding BorderThickness}" Background="{TemplateBinding Background}" SnapsToDevicePixels="true">
                            <ContentPresenter x:Name="contentPresenter" Focusable="False" HorizontalAlignment="{TemplateBinding HorizontalContentAlignment}" Margin="{TemplateBinding Padding}" RecognizesAccessKey="True" SnapsToDevicePixels="{TemplateBinding SnapsToDevicePixels}" VerticalAlignment="{TemplateBinding VerticalContentAlignment}"/>
                        </Border>
                        <ControlTemplate.Triggers>

                            <Trigger Property="IsMouseOver" Value="true">
                                <Setter Property="Background" TargetName="border" Value="{StaticResource google_icon}"/>
                            </Trigger>
                            <Trigger Property="IsPressed" Value="true">
                                <Setter Property="Background" TargetName="border" Value="{StaticResource google_icon}"/>
                            </Trigger>
                            <Trigger Property="IsEnabled" Value="false">
                            </Trigger>
                        </ControlTemplate.Triggers>
                    </ControlTemplate>
                </Setter.Value>
            </Setter>
        </Style>
    </Window.Resources>
    
        <Button Height="150" Width="150" BorderThickness="0" Style="{DynamicResource googleStyle}"></Button>
 */