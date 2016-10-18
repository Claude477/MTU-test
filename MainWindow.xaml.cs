using System.Net.NetworkInformation;
using System.Threading;

namespace mtu_test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        Thread worker;
        private void button1_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            stop = false;
            listBox1.Items.Clear();
            worker = new Thread(work);
            worker.Start();
            //work();
        }
        bool stop = false;
        void work()
        {
            try
            {
                
                //int mtu = System.Int32.Parse(textBox1.Text);
                this.Dispatcher.Invoke((System.Action)(() =>
                {
                    int mtu = System.Int32.Parse(textBox1.Text);
                
                    byte[] buffer = new byte[1500];
                    buffer = System.Text.Encoding.ASCII.GetBytes(new string('a', 1500));
                    string txt;
                    bool arvo = true, arvo2 = false, valmis = true;
                    //mtu=1500 (smaller -10) && mtu=1300 (higher +1)
                    do
                    {
                        System.Array.Resize(ref buffer, mtu);
                        arvo = GetPing(buffer);
                        txt = buffer.Length + "=" + !arvo;
                        Thread.Sleep(5);
                        //listBox1.Items.Add(txt);
                        listBox1.Items.Add(txt);
                        if (arvo) mtu = mtu - 10;
                        if (!arvo) { mtu = mtu + 1; arvo2 = true; };
                        if (arvo && arvo2) valmis = false;
                        if (stop) valmis = false;
                    } while (valmis);
                    listBox1.Items.Add(txt+":total="+(mtu+28));

                }));
            }
            catch (System.Exception)
            {
                this.Dispatcher.Invoke((System.Action)(() =>
                {
                listBox1.Items.Add("virhe");
                }));
            }
        }
        public static bool GetPing(byte[] buffer)
        {
            bool p = true;
            PingOptions options = new PingOptions();
            options.DontFragment = true;
            using (Ping ping = new Ping())
            {
                PingReply reply = ping.Send("www.google.com", 2000, buffer,options);
                if (reply != null)
                    if (reply.Status == IPStatus.Success)
                        p = reply.Options.DontFragment;
            }
            return p;
        }

        private void button2_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            stop = true;
        }
    }
}
