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
using static System.Net.Mime.MediaTypeNames;

namespace Vigenere_Cipher
{
    public partial class MainWindow : Window
    {
        char[] TurkishAlphabet = { 'A','B','C','Ç','D','E','F','G','Ğ','H','I','İ','J','K','L','M','N','O','Ö','P','R','S','Ş','T','U','Ü','V','Y','Z'};

    public MainWindow()
        {
            InitializeComponent();
        }
        void MainCypherer(object sender, RoutedEventArgs e)
        {
            //Text and key are obtained from user input
            String txt = plaintextbox.Text.ToUpper();

            String userKey = keybox.Text.ToUpper();

            //Key is generated using the keyword that the user typed.
            char[] key = keyGenerator(txt.ToCharArray(), userKey.ToCharArray());

            //Encryption or decryption method is called based on which button is clicked.
            if (sender == encryptbutton)
            {
                ciphertext.Text = encryptToCipherText(txt.ToCharArray(), key);
            }
            else if (sender == decryptbutton)
            {
                String plainText = "";
                int keyLength = userKey.Length;
                String tempKey = userKey;
                int callNumber = (txt.Length / keyLength) + 1; //how many function calls until we decrypt the text fully
                int unit = 0; // to append first unit # of characters to the key
                int x = 0; // to append the exact number of characters in the last call
                for (int i = 0; i < callNumber; i++)
                {
                    plainText = decryptToPlainText(txt.ToCharArray(), tempKey.ToArray());
                    tempKey = userKey;
                    unit++;
                    x = 0;
                    for (int j = 0; j < unit * keyLength; j++)
                    {
                        if(x<= (txt.Length-keyLength))
                        {
                            tempKey = tempKey + plainText[j];
                            x++;
                        }
                    }
                }

                decryptedtext.Text = plainText;
            }

        }
        //Key is generated based on the keyword user entered.
        char[] keyGenerator(char[] txt, char[] key)
        {
            char[] realKey = new char[0];

            realKey = key;

            int i = 0;

            //In the case where keyword is shorter than the plaintext, key is repeted to complete.
            while( realKey.Length < txt.Length)
            {
                realKey = realKey.Append(txt[i]).ToArray();
                
                if(i == txt.Length - 1)
                {
                    i = 0;
                }
                else
                {
                    i++;
                }
            }

            return realKey;
        }

        String encryptToCipherText(char[] txt, char[] key)
        {
            String cipherText = "";

            //Encrypting every character of the text one by one and adding to ciphertext.
            for(int i = 0; i < txt.Length; i++)
            {                
                int a = Array.FindIndex(TurkishAlphabet, element => element.Equals(txt[i]));
                int b = Array.FindIndex(TurkishAlphabet, element => element.Equals(key[i]));
                int c = (a + b) % 29;
                cipherText = cipherText + TurkishAlphabet[c];
            }

            return cipherText;
        }


        String decryptToPlainText(char[] cipherText, char[] key)
        {
            String plainText = "";

            //Key is generated using the keyword that the user typed.
            char[] realKey = keyGenerator(cipherText, key);

            //Decrypting every character of the text one by one and adding to the plaintext.
            for (int i = 0; i < cipherText.Length; i++)
            {
                int a = Array.FindIndex(TurkishAlphabet, element => element.Equals(cipherText[i]));
                int b = Array.FindIndex(TurkishAlphabet, element => element.Equals(realKey[i]));
                int c = (a - b + 29) % 29;
                plainText = plainText + TurkishAlphabet[c];
            }

            return plainText;
        }
    }
}