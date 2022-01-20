﻿using System;
using System.Collections.Generic;
using System.IO;

namespace project
{
    class disease
    {
        public string name;
        public List<string> daroomos = new List<string>();
        public List<string> darooman = new List<string>();
        // public Dictionary<string, string> drugsEffects = new Dictionary<string, string>();
        // public Dictionary<string, char> alergies = new Dictionary<string, char>();
        public disease(string name)
        {
            this.name = name;
        }
        // public void adddrug(drug a)
        // {
        //     //daroo.Add(a);
        //     ///
        // }
        public void deletefromMOS(string name){
            for (int i = 0; i < daroomos.Count; i++)
            {
                if (daroomos[i]==name)
                {
                    daroomos.RemoveAt(i);                    
                }
            }
        }
        public void deletefromMAN(string name){
            for (int i = 0; i < darooman.Count; i++)
            {
                if (darooman[i]==name)
                {
                    darooman.RemoveAt(i);                    
                }
            }
        }
        public void adddrugeffect(string drugef)
        {
            string[] spil = drugef.Split(",");

            if (spil[1].Contains("+") == true)
            {
                daroomos.Add(drugef);
                string[] drugname = spil[0].Split('(');
                
                long hash = drugStore.polynomialRollingHash(drugname[1])%2000;
                if (hash<0)
                {
                    hash*=-1;
                }
                drugStore.hash_table[hash][drugname[1]].goodfor.Add(this.name);
            }
            if (spil[1].Contains("-") == true)
            {
                darooman.Add(drugef);
                string[] drugname = spil[0].Split('(');

                long hash = drugStore.polynomialRollingHash(drugname[1])%2000;
                if (hash<0)
                {
                    hash*=-1;
                }
                drugStore.hash_table[hash][drugname[1]].badfor.Add(this.name);
            }
        }
    }
    class drug
    {
        public string name;
        public int price;
        public Dictionary<string, string> drugsEffects = new Dictionary<string, string>();
        // public Dictionary<string, char> alergies = new Dictionary<string, char>();
        public List<string> goodfor = new List<string>();
        public List<string> badfor = new List<string>();
        public drug(string name, int price)
        {
            this.name = name;
            this.price = price;
        }
        public void addEffect(string drugName, string Effect)
        {
            drugsEffects.Add(drugName, Effect);
        }
        // public void addAlergy(string allergy, char Effect)
        // {
        //     alergies.Add(allergy, Effect);
        // }
        public void deletefromGOOD(string name){
            for (int i = 0; i < goodfor.Count; i++)
            {
                if (goodfor[i]==name)
                {
                    goodfor.RemoveAt(i);                    
                }
            }
        }
        public void deletefromBAD(string name){
            for (int i = 0; i < badfor.Count; i++)
            {
                if (badfor[i]==name)
                {
                    badfor.RemoveAt(i);                    
                }
            }
        }
    }
    class drugStore
    {
        public static double percent=1;
        public static List<string> random_drug= new List<string>();
        public static List<string> random_disease=new List<string>();
        public static List<string> random_effect = new List<string>();
        public static long polynomialRollingHash(String str)
        {
            // P and M
            int p = 31;
            int m = (int)(1e9 + 9);
            long power_of_p = 1;
            long hash_val = 0;

            // Loop to calculate the hash value
            // by iterating over the elements of String
            for (int i = 0; i < str.Length; i++)
            {
                hash_val = (hash_val + (str[i] -
                            'a' + 1) * power_of_p) % m;
                power_of_p = (power_of_p * p) % m;
            }
            return hash_val;
        }
        public static Dictionary<string, disease>[] hash_table2 = new Dictionary<string, disease>[500];
        /// <summary>
        /// hi
        /// </summary>
        public static Dictionary<string, drug>[] hash_table = new Dictionary<string, drug>[2000];
        public drugStore()
        {
            for (int i = 0; i < 2000; i++)
            {
                hash_table[i] = new Dictionary<string, drug>();
            }
            ////////////////////////////////////////////////////////
            for (int i = 0; i < 500; i++)
            {
                hash_table2[i] = new Dictionary<string, disease>();
            }
        }
        public void init()
        {
            int random_dc =0;
            int random_dsc=0;
            int random_ec=0;
            for (int i = 0; i < 2000; i++)
            {
                hash_table[i] = new Dictionary<string, drug>();
            }
            string[] lines = System.IO.File.ReadAllLines("drugs.txt");
            foreach (string line in lines)
            {
                string[] splited = line.Split(':');
                long hash = polynomialRollingHash(splited[0].Remove(splited[0].Length - 1, 1)) % 2000;
                if (random_dc<15)
                {
                    random_drug.Add(splited[0].Remove(splited[0].Length - 1, 1));
                    random_dc++;
                }
                
                if (hash < 0)
                {
                    hash *= -1;
                }
                hash_table[hash].Add(splited[0].Remove(splited[0].Length - 1, 1), new drug(splited[0].Remove(splited[0].Length - 1, 1), int.Parse(splited[1])));
            }
            ///////////////////////////////////////////////////////////////////////////////////
            for (int i = 0; i < 500; i++)
            {
                hash_table2[i] = new Dictionary<string, disease>();
            }
            string[] lines1 = System.IO.File.ReadAllLines("alergies.txt");
            foreach (string line in lines1)
            {
                string[] splited = line.Split(':');
                long hash = polynomialRollingHash(splited[0].Remove(splited[0].Length - 1, 1)) % 500;
                disease a = new disease(splited[0].Remove(splited[0].Length - 1, 1));
                if (random_dsc<15)
                {
                    random_disease.Add(splited[0].Remove(splited[0].Length - 1, 1));
                    random_dc++;
                }
                if (hash < 0)
                {
                    hash *= -1;
                }
                string[] splited2 = splited[1].Split(";");
                for (int i = 0; i < splited2.Length; i++)
                {
                    a.adddrugeffect(splited2[i]);
                    //how hash q?
                    ///trim q?
                }
                hash_table2[hash].Add(splited[0].Remove(splited[0].Length - 1, 1), a);
            }
            /////////////////////////////////////////////////////// eeffeecctt
            string[] lines2 = System.IO.File.ReadAllLines("effects.txt");
            foreach (string line in lines2)
            {
                string[] splited = line.Split(':');
                long hash = polynomialRollingHash(splited[0].Remove(splited[0].Length - 1, 1)) % 2000;
               
                if (hash < 0)
                {
                    hash *= -1;
                }
                string[] splited2 = splited[1].Split(";");
                for (int i = 0; i < splited2.Length; i++)
                {
                    string[] newstr = splited2[i].Split('(');
                    string[] newstr1 = newstr[1].Split(')');
                    string[] final = newstr1[0].Split(',');
                    hash_table[hash][splited[0].Remove(splited[0].Length - 1, 1)].addEffect(final[0],final[1]);
                    if (random_ec<15)
                    {
                        random_effect.Add(final[1]);
                        random_ec++;
                    }
                }
            }

        }
        public void adddisease(string diseasename)
        {
            long hash = polynomialRollingHash(diseasename) % 500;
            if (hash < 0)
            {
                hash *= -1;
            }

            if (hash_table2[hash].ContainsKey(diseasename) == true)
            {
                throw new Exception("this disease is already exist!! you can not add it again!!");
            }
            else
            {
                hash_table2[hash].Add(diseasename, new disease(diseasename));

                //add random drugs
                Random rnd = new Random();
                int r_d = rnd.Next(1,15);
                hash_table2[hash][diseasename].adddrugeffect(" ("+random_drug[r_d]+",+) ");
                int r_d2 = rnd.Next(1,15);
                while(r_d2==r_d){
                    r_d2=rnd.Next(1,15);
                }
                hash_table2[hash][diseasename].adddrugeffect(" ("+random_drug[r_d2]+",-) ");
                Console.WriteLine("######## daroohaye tasadofi#######");
                Console.WriteLine("darooye tasadofie zir baraye in bimari khoob ast:");
                Console.WriteLine("     "+random_drug[r_d]);
                Console.WriteLine("darooye tasadofie zir baraye in bimari bad ast:");
                Console.WriteLine("     "+random_drug[r_d2]);



            }
        }
        public void addNewDrug(string name, int price)
        {

            //add the new drug to our objects


            long hash = polynomialRollingHash(name) % 2000;
            if (hash < 0)
            {
                hash *= -1;
            }

            if (hash_table[hash].ContainsKey(name) == true)
            {
                throw new Exception("this drug is already exist!! you can not add it again!!");
            }
            else
            {
                hash_table[hash].Add(name, new drug(name, price));

                //some random drugs for this drug effect
                Random rnd = new Random();

                int d = rnd.Next(1, 15); // returns random integers >= 0 and < 16
                int e = rnd.Next(1, 15);
                long hash_d = polynomialRollingHash(random_drug[d]) % 2000;
                if (hash_d < 0)
                {
                    hash_d *= -1;
                }
                hash_table[hash_d][random_drug[d]].addEffect(name,random_effect[e]);
                hash_table[hash][name].addEffect(random_drug[d],random_effect[e]);

                int d2 = rnd.Next(1,15);
                while(d2==d){
                    d2 = rnd.Next(1,15);
                }
                int e2 = rnd.Next(1,15);
                long hash_d2 = polynomialRollingHash(random_drug[d2]) % 2000;
                if (hash_d2 < 0)
                {
                    hash_d2 *= -1;
                }
                hash_table[hash_d2][random_drug[d2]].addEffect(name,random_effect[e2]);
                hash_table[hash][name].addEffect(random_drug[d2],random_effect[e2]);
                //write this random to user
                Console.WriteLine("######## daroohaye tasadofi#######");
                Console.WriteLine("2 darooye random ba effect hayeshan be in daroo ezafe shod:");
                Console.WriteLine("     "+random_drug[d]+" , "+random_effect[e]);
                Console.WriteLine("     "+random_drug[d2]+" , "+random_effect[e2]);

                //some random alergy for this drug alergy
                int d1 = rnd.Next(1,15);
                long hash_d1 = polynomialRollingHash(random_disease[d1])%500;
                if (hash_d1<0)
                {
                    hash_d1 *= -1;
                }
                hash_table2[hash_d1][random_disease[d1]].adddrugeffect(" ("+name+",+) ");
                
                int d3 = rnd.Next(1,15);
                long hash_d3 = polynomialRollingHash(random_disease[d3])%500;
                if (hash_d3<0)
                {
                    hash_d3 *= -1;
                }
                hash_table2[hash_d3][random_disease[d3]].adddrugeffect(" ("+name+",-) ");

                // //write this random alergy to the user
                Console.WriteLine("######## bimarihaye tasadofi#######");
                Console.WriteLine("in daroo baraye bimarie randome zir mofid ast:");
                Console.WriteLine(random_disease[d1]+"  "+" ("+name+",+) ");
                Console.WriteLine("in daroo baraye bimarie randome zir mozer ast:");
                Console.WriteLine(random_disease[d3]+"  "+" ("+name+",-) ");

            }
        }
        public void readDisease(string diseasename)
        {
            long hash = polynomialRollingHash(diseasename) % 500;
            if (hash < 0)
            {
                hash *= -1;
            }
            if (hash_table2[hash].ContainsKey(diseasename) == true)
            {
                //Console.Write(hash_table2[hash][diseasename].alergies + " : " + hash_table2[hash][diseasename].drugsEffects);


                Console.WriteLine("daroo haii ke roye in bimari asare mostbat darand:");
                // foreach (KeyValuePair<string, string> item in hash_table2[hash][diseasename].drugsEffects)
                // {
                //     Console.WriteLine(item.Key, item.Value);
                // }
                foreach(string m in hash_table2[hash][diseasename].daroomos){
                    Console.Write(m+" ");
                }
                Console.WriteLine();
                Console.WriteLine("daroo haii ke roye in bimari asare manfi darand:");
                foreach(string m in hash_table2[hash][diseasename].darooman){
                    Console.Write(m+" ");
                }
                Console.WriteLine();
            }
            else
            {

                throw new Exception("the disease is not in the drug store!!");
            }

        }
        public void deleteDisease(string diseasename)
        {
            long hash = polynomialRollingHash(diseasename) % 500;
            if (hash < 0)
            {
                hash *= -1;
            }
            if (hash_table2[hash].ContainsKey(diseasename) == true)
            {
                Console.WriteLine("in bimari az liste daroohaye zir be onvane darooye khoob hazf shod:");
                foreach (string dr in hash_table2[hash][diseasename].daroomos){
                    string[] temp = dr.Split('(');
                    string[] temp1 = temp[1].Split(',');
                    string g = temp1[0];
                    long hash2 = polynomialRollingHash(g) % 2000;
                    if (hash2 < 0)
                    {
                        hash2 *= -1;
                    }
                    hash_table[hash2][g].deletefromGOOD(diseasename);
                    Console.Write(g+" ");
                }
                Console.WriteLine();
                Console.WriteLine("in bimari az liste daroohaye zir be onvane darooye bad hazf shod:");
                foreach (string dr in hash_table2[hash][diseasename].darooman){
                    string[] temp = dr.Split('(');
                    string[] temp1 = temp[1].Split(',');
                    string g = temp1[0];
                    long hash2 = polynomialRollingHash(g) % 2000;
                    if (hash2 < 0)
                    {
                        hash2 *= -1;
                    }
                    hash_table[hash2][g].deletefromBAD(diseasename);
                    Console.Write(g+" ");
                }
                Console.WriteLine();
                hash_table2[hash].Remove(diseasename);
            }
            else
            {

                throw new Exception("the disease is not in the drug store!!");
            }

        }
        public void readDrug(string name)
        {
            long hash = polynomialRollingHash(name) % 2000;
            if (hash < 0)
            {
                hash *= -1;
            }
            if (hash_table[hash].ContainsKey(name) == true)
            {
                Console.WriteLine(hash_table[hash][name].name + " : " + (hash_table[hash][name].price)*drugStore.percent);
                Console.WriteLine("this drug is good for:");
                foreach (string item in hash_table[hash][name].goodfor)
                {
                    Console.Write(item+" ");
                }
                Console.WriteLine();
                Console.WriteLine("this drug is bad for:");
                foreach (string item in hash_table[hash][name].badfor)
                {
                    Console.Write(item+" ");
                }
                Console.WriteLine();
                Console.WriteLine("this drug effects are:");
                foreach (KeyValuePair<string, string> item in hash_table[hash][name].drugsEffects)
                {
                    Console.WriteLine("[ "+item.Key+" , " +item.Value+" ]");
                }
            }
            else
            {

                throw new Exception("the drug is not in the drug store!!");
            }

        }
        public void percentprice(double per)
        {
            percent*=(1+per);
        }
        public void deleteDrug(string drugname){
            long hash = polynomialRollingHash(drugname) % 2000;
            if (hash < 0)
            {
                hash *= -1;
            }
            if (hash_table[hash].ContainsKey(drugname) == true)
            {
                Console.WriteLine("in daroo az liste daroo haye mosbat baraye bimari haye zir hazf shod:");
                foreach (string g in hash_table[hash][drugname].goodfor )
                {
                    long h2 = polynomialRollingHash(g) % 500;
                    if (h2 < 0)
                    {
                        h2 *= -1;
                    }
                    hash_table2[h2][g].deletefromMOS(drugname);
                    Console.Write(g+" ");
                }
                Console.WriteLine();
                Console.WriteLine("in daroo az liste daroo haye manfi baraye bimari haye zir hazf shod:");
                foreach (string g in hash_table[hash][drugname].badfor )
                {
                    long h3 = polynomialRollingHash(g) % 500;
                    if (h3 < 0)
                    {
                        h3 *= -1;
                    }
                    hash_table2[h3][g].deletefromMAN(drugname);
                    Console.Write(g+" ");
                }
                Console.WriteLine();
                Console.WriteLine("in daroo az liste effect haye daroohaye zir hazf shod:");
                foreach (KeyValuePair<string,string> e in hash_table[hash][drugname].drugsEffects){
                    long hash2 = polynomialRollingHash(e.Key) % 2000;
                    if (hash2 < 0)
                    {
                        hash2 *= -1;
                    }
                    hash_table[hash2][e.Key].drugsEffects.Remove(drugname);
                    Console.WriteLine(e.Key+" ");
                }
                Console.WriteLine();
                hash_table[hash].Remove(drugname);
            }
            else
            {

                throw new Exception("the drug is not in the drug store!!");
            }
        }
    }
    class Program
    {
        public static drugStore d = new drugStore();
        static void UserActions(int flag)
        {

            if (flag == 1)
            {
                d.init();
            }
            else if (flag == 2)
            {
                d.readDisease("Dis_ebpmvoxdhl");
                d.deleteDisease("Dis_ebpmvoxdhl");
                d.readDisease("Dis_ebpmvoxdhl");
                //tadakhole darooii
            }
            else if (flag == 3)
            {
                //tadakhole darooii va bimari
            }
            else if (flag == 4)
            {
                //hazine yek noskhe daroo
                d.adddisease("testdrug");
                Console.WriteLine("#######3");
                d.deleteDisease("testdrug");

            }
            else if (flag == 5)
            {
                //afzayesh ya kahesh gheimate daroo ha
                //!!!voroodi ashari beine -1 ta +1 bayad be tabe dade shavad!!!
                d.percentprice(0.2);

            }
            else if (flag == 6)
            {
                //taghirat rooye sakhtar dade
                d.addNewDrug("testdrug",8000);

            }
            else if (flag == 7)
            {
                //search dar sakhtar dade
                int option=-1;
                
                
                while(option != 3){
                    try
                    {
                        Console.WriteLine("Enter your desired option:".PadLeft(27, ' '));
                        Console.WriteLine("1.Search a drug".PadLeft(22, ' '));
                        Console.WriteLine("2.Search a dises".PadLeft(23, ' '));
                        Console.WriteLine("3.Back to menue".PadLeft(22, ' '));
                        option = int.Parse(Console.ReadLine());
                        Console.Clear();
                    }
                    catch (System.Exception)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;

                        Console.WriteLine("Invalid input!! your input is not a number!! Enter it again!");
                        Console.ForegroundColor = ConsoleColor.White;
                        continue;
                    }
                    if(option==1){
                        string drug_name = Console.ReadLine();
                        d.readDrug(drug_name);
                    }
                    else if(option==2){

                    }
                    else if(option==3){

                    }
                    else{
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Your input is not in the correct range!! Enter it again!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                
            }
            else
            {
                throw new Exception("Invalid input!! your input is not in the correct range!!");
            }
        }
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Blue;

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("                                                            Welcome Back!");
            Console.ForegroundColor = ConsoleColor.White;
            int flag = 0;
            while (flag != 8)
            {
                Console.WriteLine();
                Console.WriteLine("Enter your desired option:".PadLeft(27, ' '));
                Console.WriteLine("1.Reading data files".PadLeft(22, ' '));
                Console.WriteLine("2.Presence or absence of drug interactions in a prescription drug".PadLeft(67, ' '));
                Console.WriteLine("3.Evaluate the presence or absence of drug allergy in a prescription with referring disease".PadLeft(93, ' '));
                Console.WriteLine("4.Calculating the price of prescription drugs".PadLeft(47, ' '));
                Console.WriteLine("5.Rising prices of drugs".PadLeft(26, ' '));
                Console.WriteLine("6.Adding or removing from the data structures".PadLeft(47, ' '));
                Console.WriteLine("7.Search".PadLeft(10, ' '));
                Console.WriteLine("8.Exit".PadLeft(8, ' '));
                try
                {

                    flag = int.Parse(Console.ReadLine());
                    Console.Clear();
                }
                catch (System.Exception)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;

                    Console.WriteLine("Invalid input!! your input is not a number!!");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }
                if (flag == 8) break;
                try
                {
                    UserActions(flag);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                }

            }
        }
    }
}


