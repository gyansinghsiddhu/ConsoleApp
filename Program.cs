using ConsoleApp.DAL;
using ConsoleApp.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net;

namespace ConsoleApp
{
    class Program
    {
        private static IConfiguration _iconfiguration;

        static void Main(string[] args)
        {

            GetAppSettingsFile();
            //Scrapping("Indonesia");
            //args[0] = "th";

            if (args.Length == 0)
            {
                args = new string[1];
                args[0] = "fb";
            }

            //if (args.Length == 0)
            //{
            //    Console.WriteLine("Choose an option from the following list:");
            //    Console.WriteLine("\tth - Thailand");
            //    Console.WriteLine("\tid - Indonesia");
            //    Console.WriteLine("\tvi - Vietnam");
            //    Console.WriteLine("\tmy - Malaysia");
            //    Console.WriteLine("\tph - Philippines");
            //    Console.WriteLine("\tsg - Singapore");
            //    Console.WriteLine("\tfb - Feedback Scrapping");
            //    Console.Write("Your option? ");
            //    args = new string[1];
            //    args[0] = Console.ReadLine();

            //}
            switch (args[0].ToString())
            {
                case "th":
                    Scrapping("Thailand");
                    break;
                case "id":
                    Scrapping("Indonesia");
                    break;
                case "vi":
                    Scrapping("Vietnam");
                    break;
                case "my":
                    Scrapping("Malaysia");
                    break;
                case "ph":
                    Scrapping("Philippines");
                    break;
                case "sg":
                    Scrapping("Singapore");
                    break;
                case "fb":
                    ScrappingFeedback();
                    break;
           
            }

            // Console.Write("Scrapping Has been Completed");
            // Console.ReadKey();

        }

        static void GetAppSettingsFile()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _iconfiguration = builder.Build();
        }


        public DateTime UnixTimeToDateTime(long unixtime)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixtime).ToLocalTime();
            return dtDateTime;
        }

        static void Scrapping(string Country)
        {


            string HName = "";
            string HImgUrl = "";
            var sDAL = new ScrapDAL(_iconfiguration);
            // sDAL.FeedbackScrapping("Thailand", "1396470", "1845298128", 4282);
            //sDAL.PostGetRecord();

            if (Country == "Thailand") { HName = "https://shopee.co.th/"; HImgUrl = "https://cf.shopee.co.th/file/"; }
            else if (Country == "Indonesia") { HName = "https://shopee.co.id/"; HImgUrl = "https://cf.shopee.co.id/file/"; }
            else if (Country == "Malaysia") { HName = "https://shopee.com.my/"; HImgUrl = "https://cf.shopee.com.my/file/"; }
            else if (Country == "Vietnam") { HName = "https://shopee.vn/"; HImgUrl = "https://cf.shopee.vn/file/"; }
            else if (Country == "Philippines") { HName = "https://shopee.ph/"; HImgUrl = "https://cf.shopee.ph/file/"; }
            else if (Country == "Singapore") { HName = "https://shopee.sg/"; HImgUrl = "https://cf.shopee.sg/file/"; }
            ScrapModel sModel = new ScrapModel();
            sModel.PromotionId = sDAL.GetPromotionID(HName);
            for (; ; )
            {

                try
                {

                    if (sModel.PromotionId != sModel.PromotionId_LS)
                    {

                        sDAL.GetRecord_AllSession(Country, HName, HImgUrl, 1);
                        sModel.PromotionId_LS = sDAL.GetPromotionID(HName);
                    }
                    else
                    {
                        sModel.PromotionId = sDAL.GetPromotionID(HName);
                        continue;
                    }
                }
                catch
                {
                    continue;
                }


            }

        }


        static void ScrappingFeedback()
        {
            var sDAL = new ScrapDAL(_iconfiguration);
            sDAL.FeedbackScrapping();
        }





    }
            }
