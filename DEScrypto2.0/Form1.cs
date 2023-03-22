using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace DEScrypto2._0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        byte[] IV = new byte[8] { 220, 106, 42, 150, 75, 35, 151, 71 };

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DES DESalg = DES.Create();

                byte[] key = new byte[8];

                if (textBox2.Text != "")
                {
                    string[] words = textBox2.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < DESalg.Key.Length; i++)
                    {
                        key[i] = Convert.ToByte(words[i]);
                    }
                }
                else
                {
                    DESalg.GenerateKey();
                    key = DESalg.Key;

                    textBox2.Text = "";
                    for (int i = 0; i < key.Length; i++)
                    {
                        textBox2.Text += key[i] + " ";
                    }

                }

                

                string sData = textBox1.Text;

                byte[] Data = EncryptTextToMemory(sData, key, IV);

                textBox3.Text = "";
                for (int i = 0; i < Data.Length; i++)
                {
                    textBox3.Text += Data[i] + " ";
                }
            }
            catch
            {

            }


        }
        public byte[] EncryptTextToMemory(string Data, byte[] Key, byte[] IV)
        {
            try
            {             
                MemoryStream mStream = new MemoryStream();

                DES DESalg = DES.Create();

                CryptoStream cStream = new CryptoStream(mStream,
                    DESalg.CreateEncryptor(Key, IV),
                    CryptoStreamMode.Write);

                byte[] toEncrypt = new ASCIIEncoding().GetBytes(Data);

                cStream.Write(toEncrypt, 0, toEncrypt.Length);
                cStream.FlushFinalBlock();

                byte[] ret = mStream.ToArray();

                cStream.Close();
                mStream.Close();

                return ret;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DES DESalg = DES.Create();

            byte[] key = new byte[8];

            string[] words = textBox5.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < DESalg.Key.Length; i++)
            {
                key[i] = Convert.ToByte(words[i]);
            }



            words = textBox6.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            byte[] bData = new byte[words.Length];

            for (int i = 0; i < words.Length; i++)
            {
                bData[i] = Convert.ToByte(words[i]);
            }     

            for (int i = 0; i < bData.Length; i++)
            {
                textBox7.Text += bData[i].ToString() + " ";
            }

            string result = DecryptTextFromMemory(bData, key, IV);

            textBox7.Text = result;

            
        }

        public static string DecryptTextFromMemory(byte[] Data, byte[] Key, byte[] IV)
        {
            try
            {
                MemoryStream msDecrypt = new MemoryStream(Data);

                DES DESalg = DES.Create();

                CryptoStream csDecrypt = new CryptoStream(msDecrypt,
                    DESalg.CreateDecryptor(Key, IV),
                    CryptoStreamMode.Read);

                byte[] fromEncrypt = new byte[Data.Length];

                csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);

                return new ASCIIEncoding().GetString(fromEncrypt);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
        }
    }
}
