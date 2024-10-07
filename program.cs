namespace HotellbokningenÖ3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Hotel hotel = new Hotel();
            hotel.Load();
            hotel.initRooms();

            while (true)
            {
                int rum;
                string namn;

                Console.Clear();

                hotel.PrintRooms();
                hotel.PrintPrices();
                hotel.PrintDailyIncome();
                Console.WriteLine("ange ditt namn: ");
                namn = Console.ReadLine();





                if (string.IsNullOrEmpty(namn) || namn.Length < 3)
                {
                    Console.WriteLine("Namn kan inte vara tomt eller innehålla sifror");
                    Console.ReadLine();
                    continue;
                }

                Console.WriteLine("Ange rum nummer");
                try
                {
                    rum = int.Parse(Console.ReadLine());

                }
                catch (Exception)
                {
                    Console.WriteLine("Input cant be null and must be in correct format");
                    continue;
                    throw;
                }



                hotel.BookRoom(rum, namn);

                string[] våning1 = new string[hotel.names.GetLength(1)];
                string[] våning2 = new string[hotel.names.GetLength(1)];
                string[] våning3 = new string[hotel.names.GetLength(1)];

                for (int i = 0; i < hotel.names.GetLength(0); i++)
                {
                    for (int j = 0; j < hotel.names.GetLength(1); j++)
                    {
                        if (i == 0)
                            våning1[j] = hotel.names[i, j];
                        else if (i == 1)
                            våning2[j] = hotel.names[i, j];
                        else if (i == 2)
                            våning3[j] = hotel.names[i, j];
                    }
                }
                File.WriteAllLines("våning1.txt", våning1);
                File.WriteAllLines("våning2.txt", våning2);
                File.WriteAllLines("våning3.txt", våning3);




                Console.Clear();
                hotel.PrintRooms();
                hotel.PrintDailyIncome();
                Console.ReadLine();
            }

        }
    }

    internal class Hotel

    {
        public bool[,] antalLedigaRum = new bool[3, 4];
        public string[,] names = new string[3, 4];
        public int[] levelPrices = { 650, 775, 995 };
        public int dailyIncome = 0;

        public void Load()
        {

            string[] våning1;
            string[] våning2;
            string[] våning3;
            if (File.Exists("våning1.txt"))
            {
                våning1 = File.ReadAllLines("våning1.txt");
                for (int i = 0; i < våning1.Length; i++)
                {
                    names[0, i] = våning1[i];
                }
            }
            if (File.Exists("våning2.txt"))
            {
                våning2 = File.ReadAllLines("våning2.txt");
                for (int i = 0; i < våning2.Length; i++)
                {
                    names[1, i] = våning2[i];
                }

            }
            if (File.Exists("våning2.txt"))
            {
                våning3 = File.ReadAllLines("våning3.txt");
                for (int i = 0; i < våning3.Length; i++)
                {
                    names[2, i] = våning3[i];
                }
            }



        }

        public void initRooms()
        {


            for (int i = 0; i < antalLedigaRum.GetLength(0); i++)
            {
                for (int j = 0; j < antalLedigaRum.GetLength(1); j++)
                {
                    if (names[i, j] != null)
                    {
                        antalLedigaRum[i, j] = false;
                    }
                    else
                    {
                        antalLedigaRum[i, j] = true;
                    }
                }
            }
        }



        public void BookRoom(int roomTwoDigits, string name)
        {
            string roomString;
            string floor;
            string roomNumber;
            int floorInt = -1;
            int roomInt = -1;

            try
            {

                roomString = roomTwoDigits.ToString();

                floor = roomString.Substring(0, 1);
                roomNumber = roomString.Substring(1);
                floorInt = int.Parse(floor);
                roomInt = int.Parse(roomNumber);
            }
            catch (FormatException)
            {
                Console.WriteLine("Input needs to be in correct format:\n" +
                    "  10, 11, 12 etc ");
                Console.ReadLine();
                throw;
            }

            try
            {
                antalLedigaRum[floorInt - 1, roomInt] = false;
                names[(floorInt - 1), roomInt] = name;
                dailyIncome += levelPrices[floorInt - 1];
            }
            catch (IndexOutOfRangeException)
            {

                Console.WriteLine("Room doesn't exist");
                Console.ReadLine();
            }
        }

        public void PrintPrices()
        {
            Console.WriteLine();
            for (int i = 0; i < levelPrices.Length; i++)
            {
                Console.WriteLine("Floor " + (i + 1) + ": " + levelPrices[i] + " SEK");
            }
            Console.WriteLine();
        }

        public void PrintDailyIncome()
        {

            Console.WriteLine("Daily income: " + dailyIncome + " SEK");
        }
        public void PrintRooms()
        {



            Console.WriteLine($"Floors \t\t\t\t\t\t  Rooms \t");

            for (int i = antalLedigaRum.GetLength(0) - 1; i >= 0; i--)
            {
                Console.Write($"{(i + 1),4}\t");


                for (int j = 0; j < antalLedigaRum.GetLength(1); j++)
                {
                    Console.Write(
                        $" {(i + 1) + j.ToString(),6}:" + $"{(antalLedigaRum[i, j] ? "Avalible" : "Booked: " + names[i, j]),4}"
                        );
                }
                Console.WriteLine();
            }
        }
    }

}
