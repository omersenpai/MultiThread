using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestFormApp
{
    public partial class Form1 : Form
    {
        public int counter = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private async void btnReadFile_Click(object sender, EventArgs e)
        {
            string data = String.Empty;

            Task<String> okuma = ReadFileAsync2();//ana threadi bloke etmesin diye thread kullanırız.
            //ek thread kullanılmıyor

            richTextBox2.Text = await new HttpClient().GetStringAsync("https://www.Google.com");//data text e yazsın öyle geç diye await koyduk.
                                                                                                //ek thread kullanılmıyor.

            data = await okuma;


            richTextBox1.Text = data;
        }

        private void btnCounter_Click(object sender, EventArgs e)
        {
            textBoxCounter.Text = counter++.ToString();
        }

        private string ReadFile()
        {
            string data = string.Empty;
            using (StreamReader s = new StreamReader("DENEME.txt"))
            {//senkron kodun calısma sekli
                Thread.Sleep(5000); //ana thread uyudugu icin başka şey yapamayız.
                data = s.ReadToEnd();

            }
            return data;

        }
        //void  Task
        //string Task<string>
        private async Task<string> ReadFileAsync() //asenkron oldugu anlasılması icin.Dosya okundugu anda başka işlemler yapabiliriz.
        {
            string data = string.Empty;
            using (StreamReader s = new StreamReader("DENEME.txt"))
            {
                Task<string> myTask = s.ReadToEndAsync();//bu metod string metod donecek.Buraada thread yok
                                                         //10 sn sürdü.15 sn sürerse işlem altta 5 saniye daha bekler.
                                                         //her zaman thread kullanılmaz.  
                                                         //bu arada istenen işlemler yapılabilir.

                await Task.Delay(5000);
                data = await myTask;
                return data;
            }

        }


        private Task<string> ReadFileAsync2()
        {
            StreamReader s = new StreamReader("DENEME.txt"); //using blogunu kaldırırsak çalışır bunda
            
                return s.ReadToEndAsync();
            
        }



        //ReadToEndAsync metodu StreamReader sınıfında bulunan bir metottur.Ve dosyanın tamamını asenkron bir şekilde okumak için kullanılır.
        //bu metot, dosyanın okunması işlemini başlatır ve işlem tamamlandığında bir Task<string> nesnesi döndürür.
        //await anahtar kelimesi, asenkron bir işlemin tamamlanmasını beklemek için kullanılır. Bir metot içinde await kullanıldığında, o noktada metotun yürütmesi durur ve işlem tamamlanana kadar bekler.
        //Bir metotda await kullanmak istiyorsanız, o metodu async olarak işaretlemelisiniz.
        //await yalnızca Task veya Task<T> döndüren metotlarla kullanılabilir
        //Özetlemek gerekirse, ReadToEndAsync gibi asenkron bir metodu await ile beklemek, metot çağrısının tamamlanmasını beklerken diğer işlemlerin yapılmasını sağlar. Bu, kullanıcı arayüzünün donmamasını ve diğer işlemlerin devam etmesini sağlar. await anahtar kelimesi, asenkron işlemleri daha etkin bir şekilde yönetmenizi ve yanıt beklerken zaman kazanmanızı sağlar.
    }
}

