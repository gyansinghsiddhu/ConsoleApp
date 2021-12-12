using ConsoleApp.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ConsoleApp.DAL
{
    public class ScrapDAL
    {
        private string CnnStr;

        ScrapModel sModel;
        DataTable objDT_Cat, objDT_ItemId  , objDT_FSDetails, objDT_Feedback , objDT_ModelID , UniqeTable_Feedback;
        DataRow objDR_Cat, objDR_ItemId, objDR_FSDetails , objDR_Feedback , objDR_ModelID;

       
        SqlConnection cnn = new SqlConnection();
        SqlTransaction tran;



        public void NewCart_Cat()
        {
            objDT_Cat = new DataTable("Cart2");
            objDT_Cat.Columns.Add("PromotionID", typeof(string));
            objDT_Cat.Columns.Add("CatID", typeof(string));
            objDT_Cat.Columns.Add("CatName", typeof(string));
            objDT_Cat.Columns.Add("CatImage", typeof(string));
            AddProduct_Cat();
        }
        public void AddProduct_Cat()
        {

            objDR_Cat = objDT_Cat.NewRow();
            objDR_Cat["PromotionID"] = sModel.PromotionId;
            objDR_Cat["CatID"] = sModel.CatId;
            objDR_Cat["CatName"] = sModel.CatName;
            objDR_Cat["CatImage"] = sModel.CatImage;
            objDT_Cat.Rows.Add(objDR_Cat);
            objDT_Cat.AcceptChanges();
        }


        public void NewCart_ItemID()
        {
            objDT_ItemId = new DataTable("Cart2");
            objDT_ItemId.Columns.Add("ItemID", typeof(string));
            objDT_ItemId.Columns.Add("CatId", typeof(string));
            objDT_ItemId.Columns.Add("FSSOLDSTATUS", typeof(bool));
            AddProduct_ItemID();
        }
        public void AddProduct_ItemID()
        {

            objDR_ItemId = objDT_ItemId.NewRow();
            objDR_ItemId["ItemID"] = sModel.ItemID;
            objDR_ItemId["CatId"] = sModel.CatId;
            objDR_ItemId["FSSOLDSTATUS"] = sModel.FSSoldOutStatus;
            objDT_ItemId.Rows.Add(objDR_ItemId);
            objDT_ItemId.AcceptChanges();
        }


        public void NewCart_FSDetails()
        {
            objDT_FSDetails = new DataTable("Cart2");

            objDT_FSDetails.Columns.Add("URL1", typeof(string));
            objDT_FSDetails.Columns.Add("SHOPID", typeof(string));
            objDT_FSDetails.Columns.Add("PRODID", typeof(string));
            objDT_FSDetails.Columns.Add("CATID", typeof(string));
            objDT_FSDetails.Columns.Add("CATName", typeof(string));
            objDT_FSDetails.Columns.Add("FSSoldOutStatus", typeof(bool));
            objDT_FSDetails.Columns.Add("PName", typeof(string));
            objDT_FSDetails.Columns.Add("sprice", typeof(string));
            objDT_FSDetails.Columns.Add("fprice", typeof(string));
            objDT_FSDetails.Columns.Add("solditm", typeof(string));
            //objDT_FSDetails.Columns.Add("FS_SLOT", typeof(string));
            //objDT_FSDetails.Columns.Add("SLOT", typeof(int));
            //objDT_FSDetails.Columns.Add("SLOT_TM", typeof(string));


            AddProduct_FSDetails();
        }
        public void AddProduct_FSDetails()
        {

            objDR_FSDetails = objDT_FSDetails.NewRow();

            objDR_FSDetails["URL1"] = sModel.ProductURL;
            objDR_FSDetails["SHOPID"] = sModel.ShopID;
            objDR_FSDetails["PRODID"] = sModel.ItemID;
            objDR_FSDetails["CATID"] = sModel.CatId;
            objDR_FSDetails["CATName"] = sModel.CatName;
            objDR_FSDetails["FSSoldOutStatus"] = sModel.FSSoldOutStatus;
            objDR_FSDetails["PName"] = sModel.ProductName;
            objDR_FSDetails["sprice"] = sModel.PriceSlash;
            objDR_FSDetails["fprice"] = sModel.PriceFS;
            objDR_FSDetails["solditm"] = sModel.FSLatestSold;

            objDT_FSDetails.Rows.Add(objDR_FSDetails);
            objDT_FSDetails.AcceptChanges();
        }


        public void NewCart_Feedback()
        {
            objDT_Feedback = new DataTable("Cart2");
            // objDT2.Columns.Add("SCRAP_ID", typeof(string));
            //objDT2.Columns.Add("LINKS", typeof(string));
            objDT_Feedback.Columns.Add("Country", typeof(string));
            objDT_Feedback.Columns.Add("ShopID", typeof(string));
            objDT_Feedback.Columns.Add("ProdID", typeof(string));
            objDT_Feedback.Columns.Add("Model_ID", typeof(string));
            objDT_Feedback.Columns.Add("Model_Name", typeof(string));
            objDT_Feedback.Columns.Add("UNIT_SOLD", typeof(int));
            objDT_Feedback.Columns.Add("MonthlySoldRef", typeof(int));
            objDT_Feedback.Columns.Add("Feedback_Ratio", typeof(Decimal));
            objDT_Feedback.Columns.Add("TOTAL_SOLD", typeof(int));
            objDT_Feedback.Columns.Add("TOTAL_VAR", typeof(int));



            AddProduct_Feedback();
        }
        public void AddProduct_Feedback()
        {

            objDR_Feedback = objDT_Feedback.NewRow();
            objDR_Feedback["Country"] = sModel.Country;
            objDR_Feedback["ShopID"] = sModel.ShopID; 
            objDR_Feedback["ProdID"] = sModel.ItemID; 
            objDR_Feedback["Model_ID"] = sModel.ModelID;
            objDR_Feedback["Model_Name"] = sModel.Model_Name;
            objDR_Feedback["UNIT_SOLD"] = sModel.UNIT_SOLD; // total; // Change to FeedbackCount
            objDR_Feedback["MonthlySoldRef"] = sModel.MonthlySold; // Change to FeedbackLimit
            double ab = Math.Round((Convert.ToDouble(sModel.UNIT_SOLD * 100) / Convert.ToDouble(sModel.MonthlySold)), 2);
            sModel.Feedback_Ratio = Convert.ToDecimal(ab);
            objDR_Feedback["Feedback_Ratio"] = sModel.Feedback_Ratio;
            objDR_Feedback["TOTAL_SOLD"] = sModel.TotalSold;  // total_sold;
            objDR_Feedback["TOTAL_Var"] = sModel.VariationUnitSold; // total_var;

            objDT_Feedback.Rows.Add(objDR_Feedback);
            objDT_Feedback.AcceptChanges();
        }


        public void NewCart_ModelID()
        {
            objDT_ModelID = new DataTable("Cart");
            objDT_ModelID.Columns.Add("Model_ID", typeof(string));
            AddProduct_ModelID();

        }
        public void AddProduct_ModelID()
        {

            objDR_ModelID = objDT_ModelID.NewRow();
            objDR_ModelID["Model_ID"] = sModel.ModelID.Trim();
            objDT_ModelID.Rows.Add(objDR_ModelID);
            objDT_ModelID.AcceptChanges();


        }


        public ScrapDAL(IConfiguration iconfiguration)
        {
            CnnStr = iconfiguration.GetConnectionString("Default");
        }

        public DateTime getCurrentTime(string Country)

        {
            if (Country == "Malaysia" || Country == "Philippines" || Country == "Singapore")
            {
                return  DateTime.Now.AddHours(1);
            }
            else
            {
                return DateTime.Now;
            }

        }


        public void GetRecord_AllSession(string Country , string HostName , string HostImage, int intTime )
        {
            
            try
            {
                string s_ResponseString = "";
                string urljsn = "";
               
                objDT_Cat = null;
                objDT_ItemId = null;
                objDT_FSDetails = null;
                sModel = new ScrapModel();
                           
                sModel.HostName = HostName;
                sModel.HostImg = HostImage;
                sModel.Country = Country;
                Console.WriteLine("FS Scrapping is Started For "+Country);

                urljsn = sModel.HostName+"api/v4/flash_sale/get_all_sessions";
                var request = WebRequest.Create(urljsn);
                try
                {
                    var response = request.GetResponse();
                    var reader = new StreamReader(response.GetResponseStream());
                    s_ResponseString = reader.ReadToEnd();

                    Console.WriteLine("FS Scrapping Receved JSON Data(All Session)"+ urljsn);
                    Console.WriteLine(s_ResponseString);
                    Console.WriteLine("End of JSON Data. Time Now is: "+DateTime.Now.ToString());

                    dynamic Sessiondata = Newtonsoft.Json.Linq.JObject.Parse(s_ResponseString);
                    sModel.PromotionId = Sessiondata.data.sessions[0].promotionid;
                    sModel.PromotionName = Sessiondata.data.sessions[0].name;
                    sModel.ProStartTime = Sessiondata.data.sessions[0].start_time;
                    sModel.ProEndTime = Sessiondata.data.sessions[0].end_time;
                    DateTimeOffset offset1 = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(sModel.ProStartTime));
                    DateTimeOffset offset2 = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(sModel.ProEndTime));
                    if(Country== "Malaysia" || Country == "Philippines" || Country == "Singapore")
                    {
                        sModel.FS_StartTime = offset1.DateTime.ToLocalTime().AddHours(1);
                        sModel.FS_EndTime = offset2.DateTime.ToLocalTime().AddHours(1);
                    }
                   
                    else if(Country == "Thailand" || Country == "Indonesia" || Country == "Vietnam")
                    {
                        sModel.FS_StartTime = offset1.DateTime.ToLocalTime();
                        sModel.FS_EndTime = offset2.DateTime.ToLocalTime();
                    }
        
                    
                    int res = DateTimeOffset.Compare(offset2, offset1);  // Comapare Two Date which is Greater if 1 then second is Greater than one
                    TimeSpan difference = offset2.Subtract(offset1);
                    int slotdiff =   difference.Hours;
                    //string[] subs = sModel.PromotionName.Split('|');
                    sModel.ScrapName = sModel.FS_StartTime.ToString("yy") + sModel.FS_StartTime.Month.ToString("00") + sModel.FS_StartTime.Day.ToString("00") + "_" + sModel.FS_StartTime.Hour.ToString("00") + "." + sModel.FS_StartTime.Minute.ToString("00");

                    if (IsRecordExistProInfo(sModel.PromotionId, sModel.Country) == false)
                        SavePromotionInfo(sModel.PromotionId, sModel.PromotionName, sModel.ProStartTime, sModel.ProEndTime, sModel.Country);
                   
                    foreach (var catDetails in Sessiondata.data.sessions[0].categories)
                    {
                        try 
                        {
                            sModel.CatId = catDetails.catid;
                            sModel.CatName = catDetails.catname;
                            sModel.CatImage = catDetails.image;
                            if (objDT_Cat == null) NewCart_Cat();
                            else AddProduct_Cat();
                            if (IsRecordExistCatInfo(sModel.PromotionId, sModel.Country, sModel.CatId) == false)
                                SaveCatInfo(sModel.PromotionId, sModel.Country, sModel.CatId, sModel.CatName, sModel.CatName, sModel.CatImage);

                        }
                        catch
                        {

                        }
                    }
                    sModel.StartDate = getCurrentTime(sModel.Country); // sModel.FS_StartTime;
                    for (int slot = 0; slot <= (slotdiff + 1); slot++)
                    {

                        try
                        {
                            int DlySlot;
                            objDT_ItemId = null;
                            objDT_FSDetails = null;

                            sModel.PromotionId_LS = GetPromotionID(sModel.HostName);
                            if (sModel.PromotionId != sModel.PromotionId_LS) break;

                            if (slot == 0) DlySlot = 0;
                            else if (slot == 1 || slot == 2 || slot == (slotdiff + 1))
                            {
                                DlySlot = 25 * 60000;  //25
                            }
                            else
                            {
                                DlySlot = 50 * 60000; //50
                            }
                            
                            Delay(DlySlot);
                            sModel.StartDate = getCurrentTime(sModel.Country);

                            sModel.PromotionId_LS = GetPromotionID(sModel.HostName);
                          
                            if (sModel.PromotionId != sModel.PromotionId_LS) break;

                            sModel.SrNO = 0;
                            urljsn = sModel.HostName + "api/v2/flash_sale/get_all_itemids?promotionid=" + sModel.PromotionId;
                            request = WebRequest.Create(urljsn);
                            try
                            {
                                response = request.GetResponse();
                                reader = new StreamReader(response.GetResponseStream());
                                s_ResponseString = reader.ReadToEnd();

                                Console.WriteLine("GET ALL ITEM ID'S JSON API " + urljsn);
                                Console.WriteLine(s_ResponseString);
                                Console.WriteLine("End of JSON Data. Time Now is: " + DateTime.Now.ToString());

                                dynamic itemdata = Newtonsoft.Json.Linq.JObject.Parse(s_ResponseString);

                                int CountItm = 0;
                                try
                                {
                                    foreach (var itmID in itemdata.data.item_brief_list)
                                    {
                                        sModel.ItemID = itmID.itemid;
                                        sModel.CatId = itmID.catid;
                                        sModel.FSSoldOutStatus = Convert.ToBoolean(itmID.is_soldout);   //   SOLD_OUT STATUS
                                        if (objDT_ItemId == null) NewCart_ItemID();
                                        else AddProduct_ItemID();
                                        CountItm++;
                                    }

                                }
                                catch { CountItm = 0; }

                                try
                                {
                                    int strt = 0;
                                    for (int i = 0; i < CountItm / 12; i++)
                                    {
                                        sModel.PromotionId_LS = GetPromotionID(sModel.HostName);
                                       
                                        if (sModel.PromotionId != sModel.PromotionId_LS) break;

                                       // Console.WriteLine(sModel.PromotionId + " Completed  Status:" + i.ToString()+"/"+ (CountItm / 12));
                                        //break; //////// REMOVE THIS AFTER TESTING 
                                        int counter = 0;
                                        string itemids = "";
                                        string JAllitemList = "";
                                        for (int j = strt; j <= CountItm; j++)
                                        {
                                            if (counter > 11) break;
                                            if (itemids != "")
                                                itemids = itemids + ",    " + objDT_ItemId.Rows[j][0].ToString();
                                            else
                                                itemids = "[" + objDT_ItemId.Rows[j][0].ToString();
                                            strt++;
                                            counter++;
                                        }
                                        JAllitemList = PostGetRecord(sModel.PromotionId, itemids + "   ]");
                                        if (JAllitemList.Length < 200) continue;
                                        dynamic JS_FS_data = Newtonsoft.Json.Linq.JObject.Parse(JAllitemList);

                                        foreach (var FSDATA in JS_FS_data.data.items)
                                        {
                                            try
                                            {
                                                sModel.ItemID = FSDATA.itemid;
                                                sModel.ShopID = FSDATA.shopid;
                                                sModel.CatId = FSDATA.flash_catid;
                                                sModel.CatName = GetCatName(sModel.PromotionId, sModel.CatId, sModel.Country);
                                                sModel.PromotionId = FSDATA.promotionid;
                                                sModel.ProductName = FSDATA.name;
                                                sModel.ProductURL = sModel.HostName + "--i." + sModel.ShopID + "." + sModel.ItemID;
                                                sModel.PriceSlash = FSDATA.price_before_discount / 100000.0;
                                                sModel.PriceFS = FSDATA.price / 100000.0;
                                                sModel.FSLatestSold = (Convert.ToInt32(FSDATA.flash_sale_stock) - Convert.ToInt32(FSDATA.stock));
                                                sModel.FSSoldOutStatus = false;
                                                if (Convert.ToInt32(FSDATA.stock) > 0)
                                                    sModel.FSSoldOutStatus = false;
                                                else sModel.FSSoldOutStatus = true;
                                                //sModel.StartDate = DateTime.Now;
                                                sModel.SrNO++;
                                                sModel.Slot = slot;
                                                if (objDT_FSDetails == null) NewCart_FSDetails();
                                                else AddProduct_FSDetails();


                                                /////////////////// PRE SCRAPPING /////////////////
                                                ///

                                            string  ProdDataStrng = GetPreScrapRecord(sModel.HostName,sModel.ShopID, sModel.ItemID);
                                            if (ProdDataStrng.Length < 200) continue;

                                                try
                                                {
                                                    dynamic Jdata = Newtonsoft.Json.Linq.JObject.Parse(ProdDataStrng);

                                                        // Category Details 
                                                    int catcnt = 0;
                                                    try
                                                    {
                                                        foreach (var cat in Jdata.data.fe_categories)
                                                        {
                                                            catcnt++;
                                                            try { if (catcnt == 1) sModel.Category1 = cat.display_name; }
                                                            catch { sModel.Category1 = "N/A"; }
                                                            try { if (catcnt == 2) sModel.Category2 = cat.display_name; }
                                                            catch { sModel.Category2 = "N/A"; }
                                                            try { if (catcnt == 3) sModel.Category3 = cat.display_name; }
                                                            catch { sModel.Category3 = "N/A"; }
                                                        }
                                                    }
                                                    catch
                                                    {
                                                        sModel.Category1 = "N/A";
                                                        sModel.Category2 = "N/A";
                                                        sModel.Category3 = "N/A";
                                                    }

                                                    sModel.ShopID = Jdata.data.shopid;
                                                    sModel.CatId = Jdata.data.catid;
                                                    sModel.ShopName = GetShopName(sModel.HostName, sModel.ShopID, sModel.Country);
                                                    sModel.ItemID = Jdata.data.itemid;
                                                    sModel.ProductName = Jdata.data.name;
                                                    sModel.PriceFS = Jdata.data.price / 100000.0; ;
                                                     
                                                    
                                                    ////////////////PRICE RANGE CALCULATION

                                                    sModel.PriceMin = Jdata.data.price_min / 100000.0; 
                                                    sModel.PriceMax = Jdata.data.price_max / 100000.0;
                                                    if (sModel.PriceMin != sModel.PriceMax)
                                                        sModel.PriceRange = ((sModel.PriceMin + sModel.PriceMax) / 2).ToString("0.00");
                                                    //sModel.PriceRange = (sModel.PriceMin.ToString() + "-" + sModel.PriceMax.ToString()).ToString();
                                                    else sModel.PriceRange = sModel.PriceFS.ToString();
                                                     
                                                    ////////////////////////////////////////////////////


                                                    sModel.Star = Jdata.data.item_rating.rating_star;
                                                    sModel.Star = Math.Round(Convert.ToDecimal(sModel.Star),2);
                                                    sModel.Rating = Jdata.data.item_rating.rating_count[0];
                                                    sModel.TotalSold = Jdata.data.historical_sold;
                                                    sModel.MonthlySold = Jdata.data.sold;
                                                    sModel.SellerType = Jdata.data.shipping_icon_type;
                                                    if (sModel.SellerType == "0") sModel.SellerType = "Local"; else sModel.SellerType = "Cross Border";
                                                    sModel.UnixCTime = Jdata.data.ctime;

                                                    // VARIATION DETAILS ...........................

                                                    int index2 = 0;
                                                    try { foreach (var trVar in Jdata.data.models) index2++; }
                                                    catch { index2 = 0; }
                                                            
                                                    foreach (var variation in Jdata.data.models)
                                                    {
                                                            if (index2 > 1)
                                                            {

                                                                sModel.VariationName = variation.name;
                                                                sModel.VariationPrice = variation.price / 100000.0;
                                                                sModel.VariationBal = variation.normal_stock;
                                                                sModel.ModelID = variation.modelid;
                                                                sModel.PriceSlash = variation.price_before_discount / 100000.0;
                                                                sModel.AvailableStock =  variation.stock;
                                                                sModel.AllocatedStock = variation.price_stocks[0].allocated_stock;

                                                                int nso = variation.extinfo.tier_index[0];
                                                                sModel.VariationImageURL = sModel.HostImg + Jdata.data.tier_variations[0].images[nso];
                                                                //Console.WriteLine("If Record Exit In PreScrp Table then Data Will Not Save into Database.");
                                                                
                                                                    if (IsRecordExistPreScrap(sModel.ModelID, sModel.Country, sModel.ShopID, sModel.ItemID, sModel.ScrapName) == false)
                                                                        SavePreScrap(sModel.SrNO.ToString(), sModel.ScrapName, sModel.ProductURL, sModel.CatName, sModel.ProductName, sModel.PriceSlash, sModel.PriceFS, sModel.FSLatestSold.ToString(), sModel.ShopID, sModel.ItemID, sModel.Country, sModel.PriceSlash, sModel.PriceFS, getCurrentTime(sModel.Country).ToString(), sModel.PriceRange, sModel.Category1, sModel.Category2, sModel.Category3, sModel.SellerType, sModel.ShopName, sModel.Star, sModel.Rating, sModel.TotalSold, sModel.MonthlySold, sModel.VariationName, sModel.VariationPrice, sModel.VariationImageURL, sModel.UNIT_SOLD, Convert.ToInt32(sModel.MonthlySold), Convert.ToInt32(sModel.TotalSold), sModel.VariationPrice, sModel.VariationBal, sModel.PriceMin, sModel.PriceMax, sModel.UnixCTime.ToString(), sModel.ModelID, sModel.CatId);
                                                                
                                                                   if (IsRecordExistFSMovement(sModel.ScrapName, sModel.Country, sModel.ItemID, sModel.ShopID, sModel.ModelID, sModel.Slot) == false)
                                                                    SaveVariationStockMovement(sModel.ScrapName, sModel.Country, sModel.ItemID, sModel.ShopID, sModel.ModelID, sModel.Slot, sModel.AllocatedStock, sModel.AvailableStock, getCurrentTime(sModel.Country).ToString());
                                                           
                                                            }
                                                            else
                                                            {

                                                                sModel.VariationName = "";
                                                                sModel.VariationImageURL = sModel.HostImg + "" + Jdata.data.image;
                                                                sModel.VariationPrice = Jdata.data.price / 100000.0;
                                                                sModel.VariationBal = Jdata.data.normal_stock;
                                                                sModel.ModelID = variation.modelid;
                                                                sModel.AvailableStock = Jdata.data.stock;
                                                                sModel.AllocatedStock = variation.price_stocks[0].allocated_stock;

                                                              
                                                                    if (IsRecordExistPreScrap(sModel.ModelID, sModel.Country, sModel.ShopID, sModel.ItemID, sModel.ScrapName) == false)
                                                                        SavePreScrap(sModel.SrNO.ToString(), sModel.ScrapName, sModel.ProductURL, sModel.CatName, sModel.ProductName, sModel.PriceSlash, sModel.PriceFS, sModel.FSLatestSold.ToString(), sModel.ShopID, sModel.ItemID, sModel.Country, sModel.PriceSlash, sModel.PriceFS, getCurrentTime(sModel.Country).ToString(), sModel.PriceRange, sModel.Category1, sModel.Category2, sModel.Category3, sModel.SellerType, sModel.ShopName, sModel.Star, sModel.Rating, sModel.TotalSold, sModel.MonthlySold, sModel.VariationName, sModel.VariationPrice, sModel.VariationImageURL, sModel.UNIT_SOLD, Convert.ToInt32(sModel.MonthlySold), Convert.ToInt32(sModel.TotalSold), sModel.VariationPrice, sModel.VariationBal, sModel.PriceMin, sModel.PriceMax, sModel.UnixCTime.ToString(), sModel.ModelID, sModel.CatId);
                                                                
                                                                if (IsRecordExistFSMovement(sModel.ScrapName, sModel.Country, sModel.ItemID, sModel.ShopID, sModel.ModelID, sModel.Slot) == false)
                                                                    SaveVariationStockMovement(sModel.ScrapName, sModel.Country, sModel.ItemID, sModel.ShopID, sModel.ModelID, sModel.Slot, sModel.AllocatedStock, sModel.AvailableStock, getCurrentTime(sModel.Country).ToString());
                                                           
                                                            }

                                                        }

                                                        sModel.PriceSlash = Jdata.data.price_min_before_discount / 100000.0;

                                                        if (slot != 0)
                                                        {
                                                            if (IsRecordExist(sModel.ScrapName, sModel.Country, sModel.ShopID, sModel.ItemID) == true)
                                                            {
                                                                if (IsRecordExistNull(sModel.ScrapName, sModel.Country, sModel.ShopID, sModel.ItemID, slot) == true)
                                                                    UpdatePostScrap(sModel.Slot, sModel.Country, sModel.StartDate.ToString(), sModel.FSLatestSold.ToString(), sModel.ScrapName, sModel.ShopID, sModel.ItemID);
                                                            }
                                                            else SavePostScrap(sModel.SrNO.ToString(), sModel.ScrapName, sModel.ProductURL, sModel.CatName, sModel.ProductName, sModel.PriceSlash.ToString(), sModel.PriceFS.ToString(), sModel.FSLatestSold.ToString(), sModel.ShopID, sModel.ItemID, sModel.Country, sModel.PriceSlash, sModel.PriceFS, sModel.Slot, sModel.StartDate.ToString());
                                                        }

                                                        sModel.FSLatestSold = 0;

                                                }
                                                catch (Exception ex) { }

                                                ////////////////////END PRE SCRAPPING ///////////////////////////
                                                ///
                                            }
                                            catch (WebException ex)
                                            {
                                                string ab = ex.ToString();
                                            }

                                        }
                                        //Console.Clear();
                                       
                                    }
                                }
                                catch (WebException ex)
                                {
                                    string ab = ex.ToString();
                                }
                           
                            }
                            catch (WebException ex)
                            {
                                string ab = ex.ToString();
                            }

                        }
                        catch
                        {

                        }
                    }

                }
                catch (WebException ex)
                {

                   // Console.WriteLine("ERRO.");
                }

            }
            catch
            {

                //Console.WriteLine("ERRO.");
            }

        }


        private string PostGetRecord(string promotionid, string itemids)
        {
            string s_ResponseString = "";
            try
            {
                string urljsn = sModel.HostName + "api/v2/flash_sale/flash_sale_batch_get_items";
                WebRequest _request = WebRequest.Create(urljsn);
                _request.Method = "POST";
                _request.ContentType = "application/json;charset=UTF-8";
                using (var streamWriter = new StreamWriter(_request.GetRequestStream()))
                {
                    string limit = "12";
                    string json = "{\"promotionid\":" + promotionid + ",  \"itemids\":" + itemids + ",     \"limit\":" + limit + "}";
                    streamWriter.Write(json);

                }
                var httpResponse = (HttpWebResponse)_request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    s_ResponseString = streamReader.ReadToEnd();
                    Console.WriteLine("GET POST DETAILS OF FS DATA " + urljsn);
                    Console.WriteLine(s_ResponseString);
                    Console.WriteLine("End of SHOP DETAIL API. Time Now is: " + DateTime.Now.ToString()+ " " + urljsn);
                    return s_ResponseString;

                }

            }
            catch
            {
                return s_ResponseString;
            }
            // Creating a web request using a URL that can response a post.   


        }

        ///  FEEDBACK SCRAPPING FUNCTION 

        public void FeedbackScrapping(string cntry, string prdid )
        {
        for (; ; )
        {
                try
                {

                    cnn.ConnectionString = CnnStr;
                    //Console.Clear();
                    string country; string shopid; string prodid; string HostName=""; string Qry;
                    DataTable dt = new DataTable();

                    //Check Product ID Not Found In Database 
                    Qry = "Select ImportToCountry,ShopID,ProductID from PurchasingInfo Where UserID !=9  except select Country , ShopID , prodid from FeedbackInfo";
                    
                    if (cntry != "" && prdid != "") DeleteFeedbackData(cntry, prdid);
                    if (cnn.State == ConnectionState.Closed) cnn.Open();
                    SqlDataAdapter Sqldbda = new SqlDataAdapter(Qry, cnn);
                    Sqldbda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int MonthlySold = 0;
                            country = dt.Rows[i][0].ToString();
                            shopid = dt.Rows[i][1].ToString();
                            prodid = dt.Rows[i][2].ToString();

                            //Check Product ID Have Price Range or Not 
                            if (IsRecordExist_PriceRange(country, shopid, prodid) == false) continue;

                            UniqeTable_Feedback = null;
                            objDT_ModelID = null;

                            if (country == "Thailand")  HostName = "https://shopee.co.th/"; 
                            else if (country == "Indonesia")  HostName = "https://shopee.co.id/"; 
                            else if (country == "Malaysia")  HostName = "https://shopee.com.my/"; 
                            else if (country == "Vietnam")  HostName = "https://shopee.vn/"; 
                            else if (country == "Philippines")  HostName = "https://shopee.ph/"; 
                            else if (country == "Singapore")  HostName = "https://shopee.sg/"; 
                            

                            Console.WriteLine("FEEDBACK SCRAPPING  " + prodid + " START TIME " + getCurrentTime(country));

                            DateTime strtime = DateTime.Now;
                            sModel = new ScrapModel();
                            try
                            {
                                sModel.Country = country;
                                sModel.ShopID = shopid;
                                sModel.ItemID = prodid;
                                sModel.HostName = HostName;
                                 MonthlySold = Convert.ToInt32(GetRating(sModel.ShopID, sModel.ItemID, sModel.HostName));
                                if (MonthlySold > 3009) MonthlySold = 3009;

                                int limit = 59;
                                int ofset = 0;
                                int countFeedback = 0;
                                int totalFeedback = MonthlySold;
                                int lopcount = totalFeedback / limit;

                                string s_ResponseString = "";
                                string urljsn = "";
                                try
                                {
                                    for (int fb = 0; fb <= lopcount; fb++)
                                    {

                                        urljsn = sModel.HostName + "api/v2/item/get_ratings?itemid=" +
                                            sModel.ItemID + "&shopid=" + sModel.ShopID + "&limit=" + limit + "&offset=" + ofset + "";
                                        
                                        Console.WriteLine("GET RATING DETAILS OF ITEM " + urljsn);
                                        Console.WriteLine(s_ResponseString);
                                        Console.WriteLine("End of RATING DETAILS API for Limit "+limit+" Time Now is: " + DateTime.Now.ToString() + " " + urljsn);
                                        
                                        var request = WebRequest.Create(urljsn);
                                        try
                                        {
                                            var response = request.GetResponse();
                                            var reader = new StreamReader(response.GetResponseStream());
                                            s_ResponseString = reader.ReadToEnd();
                                            dynamic ratingData = Newtonsoft.Json.Linq.JObject.Parse(s_ResponseString);

                                            for (int m = 0; m < limit; m++)
                                            {
                                                try
                                                {
                                                    if (countFeedback >= MonthlySold) break;
                                                    sModel.ModelID = ratingData.data.ratings[m].product_items[0].modelid;
                                                    sModel.MonthlySold = MonthlySold;
                                                    if (objDT_ModelID == null) NewCart_ModelID();
                                                    else AddProduct_ModelID();
                                                    countFeedback++;
                                                }
                                                catch (Exception ex)
                                                {
                                                    break;
                                                }

                                            }

                                        }
                                        catch (WebException ex)
                                        {
                                            break;

                                        }

                                        ofset = ofset + limit;
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                            }

                            catch (Exception)
                            {
                                //break;
                                //continue;
                            }

                            try
                            {
                                int counter = 0;
                                objDT_Feedback = null;
                                UniqeTable_Feedback = null;
                                UniqeTable_Feedback = objDT_ModelID.DefaultView.ToTable(true, "Model_ID");
                                string JsonShopData = GetModelName(sModel.ShopID, sModel.ItemID, sModel.HostName);
                                if (JsonShopData.Length < 200) continue;
                                dynamic Jdata = Newtonsoft.Json.Linq.JObject.Parse(JsonShopData);

                                for (int u = 0; u <= UniqeTable_Feedback.Rows.Count - 1; u++)
                                {
                                    for (int k = 0; k <= objDT_ModelID.Rows.Count - 1; k++)
                                    {
                                        try
                                        {
                                            if (UniqeTable_Feedback.Rows[u][0].ToString() == objDT_ModelID.Rows[k][0].ToString())
                                            {
                                                counter++;
                                            }
                                        }
                                        catch { }
                                    }

                                    sModel.ModelID = UniqeTable_Feedback.Rows[u][0].ToString();
                                    sModel.UNIT_SOLD = counter;

                                   //  =========================== FROM PROUDCT API SCRAPPING GET MODEL NAME ( Live Model Name) =============================

                                    try
                                    {
                                        
                                        string LiveModelId = "";
                                        sModel.Model_Name = ""; sModel.Model_Price = 0;
                                        foreach (var trVar in Jdata.data.models)
                                        {
                                            LiveModelId = trVar.modelid;
                                            if (LiveModelId == sModel.ModelID)
                                            {
                                                sModel.Model_Name = trVar.name;
                                                sModel.Model_Price = trVar.price / 100000.0;
                                                break;
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        sModel.Model_Name = "N/A"; sModel.Model_Price = 0;
                                    }
                                   

                                    if (objDT_Feedback == null || u == 0)
                                    {
                                        if ((sModel.Model_Name != "N/A"))
                                        {

                                            sModel.TotalSold = sModel.TotalSold + sModel.UNIT_SOLD;
                                            sModel.VariationUnitSold = UniqeTable_Feedback.Rows.Count;
                                            if (objDT_Feedback == null)
                                                NewCart_Feedback();
                                            else AddProduct_Feedback();
                                            if (IsRecordExistFeedback(sModel.ModelID, sModel.Country, sModel.ShopID, sModel.ItemID) == false)
                                                SaveFeedbackData(sModel.Country, sModel.ShopID, sModel.ItemID, sModel.ModelID, sModel.Model_Name, sModel.UNIT_SOLD, sModel.MonthlySold, sModel.Feedback_Ratio, sModel.Model_Price);
                                        }
                                    }
                                    else
                                    {
                                        if ((sModel.Model_Name != "N/A"))
                                        {
                                            sModel.TotalSold = sModel.TotalSold + sModel.UNIT_SOLD;
                                            sModel.VariationUnitSold = UniqeTable_Feedback.Rows.Count;
                                            AddProduct_Feedback();
                                            if (IsRecordExistFeedback(sModel.ModelID, sModel.Country, sModel.ShopID, sModel.ItemID) == false)
                                                SaveFeedbackData(sModel.Country, sModel.ShopID, sModel.ItemID, sModel.ModelID, sModel.Model_Name, sModel.UNIT_SOLD, sModel.MonthlySold, sModel.Feedback_Ratio, sModel.Model_Price);
                                        }

                                    }
                                    counter = 0;
                                    sModel.TotalSold = 0;
                                    sModel.UNIT_SOLD = 0;
                                }
                              
                                //string endtime = " Feedback Scrapping StartTime" + strtime + " END TIME : " + DateTime.Now;
                                //Console.WriteLine(endtime);
                                //Console.ReadKey();
                            }
                            catch(Exception ex) { }

                            Console.WriteLine("FEEDBACK SCRAPPING  " + prodid + " END TIME " + getCurrentTime(country));

                        }

                    }
                    else cnn.Close();
                }
                catch
                {
                    cnn.Close();
                }
                if(cntry != "" && prdid !="")
                {
                    Console.WriteLine("Mannual Feedback Scrapping Has been Completed");
                    break;
                }
                //ChckFeedback();


        }
        }



        private void ChckFeedback()
        {
            try
            {
                string country; string shopid; string prodid; string Qry;
                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();
                Qry = "select ImportToCountry as Country, ShopID, ProductID , Count(*) as VarCount from PurchasingInfo P inner join PurchasingInfoVariation PV on P.PurchaseKey = PV.PurchaseKey where P.UserID != 9 group by ImportToCountry, ShopID, ProductID except select Country as Country, ShopID , prodid , Count(*) as VarCount  from FeedbackInfo group by Country , ShopID , prodid";
                if (cnn.State == ConnectionState.Closed) cnn.Open();
                SqlDataAdapter Sqldbda = new SqlDataAdapter(Qry, cnn);
                Sqldbda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        country = dt.Rows[i][0].ToString();
                        shopid = dt.Rows[i][1].ToString();
                        prodid = dt.Rows[i][2].ToString();
                        Qry = "select * from (select  Count(*) as VarCount from PurchasingInfo P inner join PurchasingInfoVariation PV on P.PurchaseKey = PV.PurchaseKey and P.ProductID ='" + prodid + "' and Country = '" + country + "' and ShopID='" + shopid + "') as Qry1 where VarCount > (select Count(*) as VarCount from FeedbackInfo Where ProdID ='" + prodid + "' and Country = '" + country + "' and ShopID='" + shopid + "')";
                        Sqldbda = new SqlDataAdapter(Qry, cnn);
                        Sqldbda.Fill(dt2);
                        if (dt2.Rows.Count > 0)
                        {
                            DeleteFeedbackData(country, prodid);
                        }
                        dt2.Clear();
                    }

                }
                else
                {
                    cnn.Close();
                }
            }
            catch(Exception ex)
            {

            }
        }



        private string GetModelName(string SPID, string PRID , string HostName)
        {
            string s_ResponseString = "";
            string urljsn = "";
            try
            {

                urljsn = HostName + "api/v4/item/get?itemid=" + sModel.ItemID + "&shopid=" + sModel.ShopID;

                var request = WebRequest.Create(urljsn);

                try
                {
                    var response = request.GetResponse();
                    var reader = new StreamReader(response.GetResponseStream());
                    s_ResponseString = reader.ReadToEnd();
                }
                catch (WebException ex)
                {

                    return s_ResponseString;

                }

                return s_ResponseString;
            }
            catch
            {
                return s_ResponseString;
            }

        }

        public string GetPromotionID(string HostName)
        {
            string s_ResponseString = "";
            string urljsn = "";
            try
            {

               
                urljsn = HostName + "api/v4/flash_sale/get_all_sessions";
                var request = WebRequest.Create(urljsn);

                try
                {
                    var response = request.GetResponse();
                    var reader = new StreamReader(response.GetResponseStream());
                    s_ResponseString = reader.ReadToEnd();

                    dynamic Prodata = Newtonsoft.Json.Linq.JObject.Parse(s_ResponseString);
                    s_ResponseString = Prodata.data.sessions[0].promotionid;
                }
                catch (WebException ex)
                {

                    return s_ResponseString;

                }

                return s_ResponseString;
            }
            catch
            {
                return s_ResponseString;
            }

        }


        public string GetRating(string shopid , string prodid, string HostName)
        {
            string s_ResponseString = "";
            string urljsn = "";
            try
            {

                urljsn = HostName+"api/v2/item/get_ratings?itemid=" +
                    prodid + "&shopid=" + shopid + "&limit=59&offset=0";

                var request = WebRequest.Create(urljsn);

                try
                {
                    var response = request.GetResponse();
                    var reader = new StreamReader(response.GetResponseStream());
                    s_ResponseString = reader.ReadToEnd();

                    dynamic Prodata = Newtonsoft.Json.Linq.JObject.Parse(s_ResponseString);
                    s_ResponseString = Prodata.data.item_rating_summary.rating_total;
                }
                catch (WebException ex)
                {

                    return s_ResponseString;

                }

                return s_ResponseString;
            }
            catch
            {
                return s_ResponseString;
            }

        }

        private string GetPreScrapRecord(string HostName, string SPID, string PRID)
        {
            string s_ResponseString = "";
            string urljsn = "";
            try
            {


                urljsn = HostName+"api/v4/item/get?itemid=" + PRID + "&shopid=" + SPID; 
                var request = WebRequest.Create(urljsn);
                try
                {
                    var response = request.GetResponse();
                    var reader = new StreamReader(response.GetResponseStream());
                    s_ResponseString = reader.ReadToEnd();

                    Console.WriteLine("GET ITEM DETAILS of PartiCular ITEM API  " + urljsn);
                    Console.WriteLine(s_ResponseString);
                    Console.WriteLine("End of JSON Data. Time Now is: " + DateTime.Now.ToString());
                }
                catch (WebException ex)
                {
                    return s_ResponseString;
                }
                return s_ResponseString;
            }
            catch
            {
                return s_ResponseString;
            }

        }

        private string GetShopName(string HostName, string SPID, string Country)
        {
            string s_ResponseString = "";
            string shopJsonData = ""; string topSaleJsonData = "";
            string urljsn = "";
            try
            {
                urljsn = HostName + "api/v4/shop/get_shop_detail?shopid=" + SPID;
                var request = WebRequest.Create(urljsn);
                try
                {
                    var response = request.GetResponse();
                    var reader = new StreamReader(response.GetResponseStream());
                    shopJsonData = reader.ReadToEnd();

                    dynamic Shopdata = Newtonsoft.Json.Linq.JObject.Parse(shopJsonData);
                    s_ResponseString = Shopdata.data.name;

                   
                    urljsn = HostName + "api/v4/search/search_items?by=sales&limit=10&match_id=" + SPID + "&newest=0&order=desc&page_type=shop";
                    request = WebRequest.Create(urljsn);
                    response = request.GetResponse();
                    reader = new StreamReader(response.GetResponseStream());
                    topSaleJsonData = reader.ReadToEnd();
                    dynamic TopSaleJData = Newtonsoft.Json.Linq.JObject.Parse(topSaleJsonData);


                    Console.WriteLine("GET SHOP DETAILS API  " + urljsn);
                    Console.WriteLine(s_ResponseString);
                    Console.WriteLine("End of SHOP DETAIL API. Time Now is: " + DateTime.Now.ToString()+ " " + urljsn);


                    if (IsRecordExist_ShopDATA(Country, SPID) == false) 
                        SaveShopData(Country, SPID, shopJsonData, s_ResponseString, topSaleJsonData);
                    else UpdateShopData(Country, SPID, shopJsonData, s_ResponseString, topSaleJsonData);


                }
                catch (WebException ex)
                {

                    return s_ResponseString;

                }

                return s_ResponseString;
            }
            catch(Exception ex)
            {
                return s_ResponseString;
            }

        }

        //====================================== SQL Execute FUNCTION =================================

        public void DeleteFeedbackData(string cntry, string PRID)
        {
            try
            {

               // cnn.ConnectionString = CnnStr;

                try
                {
                    if (cnn.State == ConnectionState.Closed) cnn.Open();
                    tran = cnn.BeginTransaction();
                    string Qry = "Delete From  FeedbackInfo Where Country = '"+ cntry + "' and  ProdID = '"+ PRID + "'";
                    SqlCommand Comm2 = new SqlCommand(Qry, cnn);
                    Comm2.Transaction = tran;
                    Comm2.CommandTimeout = 0;
                    Comm2.ExecuteNonQuery();
                    Comm2.Dispose();
                    tran.Commit();
                    cnn.Close();
                    Comm2.Parameters.Clear();
                }
                catch (SqlException ex)
                {
                    tran.Rollback();
                    cnn.Close();

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    cnn.Close();
                }



            }
            catch
            {



            }
        }

        public void SaveFeedbackData(string cntry, string SHID, string PRID, string ModelID, string ModelName, int UntSold, decimal MSoldRef, decimal FRatio, decimal ModelPrice)
        {

            try
            {

               // cnn.ConnectionString = CnnStr;

                try
                {
                    if (cnn.State == ConnectionState.Closed) cnn.Open();
                    tran = cnn.BeginTransaction();

                    //string Qry = "Insert Into Shop_Details values ('" + SHID + "', '" + cntry + "', N'" + APIDATA + "' , N'"+shpName+"' )";

                    string Qry = "Insert into FeedbackInfo(Country ,ShopID , ProdID , ModelID , ModelName , FeedbackCount , FeedbackLimit , FeedbackRatio,ModifiedDate, ModelPrice) values (@Country ,@ShopID , @ProdID , @ModelID , @ModelName , @FeedbackCount , @FeedbackLimit , @FeedbackRatio, @ModifiedDate, @ModelPrice)";
                    SqlCommand Comm2 = new SqlCommand(Qry, cnn);
                    Comm2.Parameters.Add("@Country", SqlDbType.NVarChar, 50).Value = cntry;
                    Comm2.Parameters.Add("@ShopID", SqlDbType.NVarChar, 50).Value = SHID;
                    Comm2.Parameters.Add("@ProdID", SqlDbType.NVarChar, 50).Value = PRID;
                    Comm2.Parameters.Add("@ModelID",SqlDbType.BigInt).Value = Convert.ToInt64(ModelID);
                    Comm2.Parameters.Add("@ModelName", SqlDbType.NVarChar, 250).Value = ModelName;
                    Comm2.Parameters.Add("@FeedbackCount", SqlDbType.Int).Value = UntSold;
                    Comm2.Parameters.Add("@FeedbackLimit", SqlDbType.Decimal).Value = MSoldRef;
                    Comm2.Parameters.Add("@FeedbackRatio", SqlDbType.Decimal).Value = FRatio;
                    Comm2.Parameters.Add("@ModifiedDate", SqlDbType.DateTime).Value = getCurrentTime(cntry);
                    Comm2.Parameters.Add("@ModelPrice", SqlDbType.Decimal).Value = ModelPrice;

                    Comm2.Transaction = tran;
                    Comm2.CommandTimeout = 0;
                    Comm2.ExecuteNonQuery();
                    Comm2.Dispose();
                    tran.Commit();
                    cnn.Close();
                    Comm2.Parameters.Clear();
                }
                catch (SqlException ex)
                {
                    tran.Rollback();
                    cnn.Close();

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    cnn.Close();
                }



            }
            catch
            {

               

            }
            //MessageBox.Show("Data updated successfully", "Response", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }


        public void SavePromotionInfo(string PromotionID, string PromotionName, string Start_Time, string End_Time, string Country)
        {
         
                cnn.ConnectionString = CnnStr;
                try
                {
                    if (cnn.State == ConnectionState.Closed) cnn.Open();
                    tran = cnn.BeginTransaction();
                    tran = cnn.BeginTransaction();

                    string Qry = "Insert into PromotionInfo(PromotionID,PromotionName,Start_Time,End_Time,Country) values (@PromotionID,@PromotionName,@Start_Time,@End_Time ,@Country)";
                    SqlCommand Comm2 = new SqlCommand(Qry, cnn);

                Comm2.Parameters.Add("@PromotionID", SqlDbType.NVarChar, 50).Value = PromotionID;
                Comm2.Parameters.Add("@PromotionName", SqlDbType.NVarChar, 50).Value = PromotionName;
                Comm2.Parameters.Add("@Start_Time", SqlDbType.NVarChar, 50).Value = Start_Time;
                Comm2.Parameters.Add("@End_Time", SqlDbType.NVarChar, 50).Value = End_Time;
                Comm2.Parameters.Add("@Country", SqlDbType.NVarChar, 50).Value = Country;
                   
                    Comm2.Transaction = tran;
                    Comm2.CommandTimeout = 0;
                    Comm2.ExecuteNonQuery();
                    Comm2.Dispose();
                    tran.Commit();
                    cnn.Close();
                    Comm2.Parameters.Clear();
                }
                catch (SqlException ex)
                {
                    tran.Rollback();
                    cnn.Close();

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    cnn.Close();
                }

          

        }


        public void SaveCatInfo(string PromotionID,  string Country , string CatID, string CatName, string CatDisplayName ,string CatImage)
        {

            cnn.ConnectionString = CnnStr;
            try
            {
                if (cnn.State == ConnectionState.Closed) cnn.Open();
                tran = cnn.BeginTransaction();

                string Qry = "Insert into CategoryInfo(PromotionID ,Country , CatID , CatName ,CatDisplayName ,CatImageID) values (@PromotionID ,@Country , @CatID , @CatName ,@CatDisplayName ,@CatImageID)";
                SqlCommand Comm2 = new SqlCommand(Qry, cnn);

                Comm2.Parameters.Add("@PromotionID", SqlDbType.NVarChar, 50).Value = PromotionID;
                Comm2.Parameters.Add("@Country", SqlDbType.NVarChar, 50).Value = Country;
                Comm2.Parameters.Add("@CatID", SqlDbType.NVarChar, 50).Value = CatID;
                Comm2.Parameters.Add("@CatName", SqlDbType.NVarChar, 250).Value = CatName;
                Comm2.Parameters.Add("@CatDisplayName", SqlDbType.NVarChar, 250).Value = CatDisplayName;
                Comm2.Parameters.Add("@CatImageID", SqlDbType.NVarChar, 250).Value = CatImage;


                Comm2.Transaction = tran;
                Comm2.CommandTimeout = 0;
                Comm2.ExecuteNonQuery();
                Comm2.Dispose();
                tran.Commit();
                cnn.Close();
                Comm2.Parameters.Clear();
            }
            catch (SqlException ex)
            {
                tran.Rollback();
                cnn.Close();

            }
            catch (Exception ex)
            {
                tran.Rollback();
                cnn.Close();
            }



        }


        public void SaveShopData(string cntry, string SHID, string APIDATA, string shpName, string topSale)
        {

            try
            {

                cnn.ConnectionString = CnnStr;

                try
                {
                    if (cnn.State == ConnectionState.Closed) cnn.Open();
                    tran = cnn.BeginTransaction();

                    string Qry = "Insert into Shop_Details(ShopID,Country,ShopDetails_Json,Shop_Name,TopSales_Json) values (@ShopID,@Country,@ShopDetails_Json,@Shop_Name, @TopSales_Json)";
                    SqlCommand Comm2 = new SqlCommand(Qry, cnn);
                    Comm2.Parameters.Add("@ShopID", SqlDbType.NVarChar, 255).Value = SHID;
                    Comm2.Parameters.Add("@Country", SqlDbType.NVarChar, 255).Value = cntry;
                    Comm2.Parameters.Add("@ShopDetails_Json", SqlDbType.NVarChar).Value = APIDATA;
                    Comm2.Parameters.Add("@Shop_Name", SqlDbType.NVarChar, 255).Value = shpName;
                    Comm2.Parameters.Add("@TopSales_Json", SqlDbType.NVarChar, 255).Value = topSale;
                    Comm2.Transaction = tran;
                    Comm2.CommandTimeout = 0;
                    Comm2.ExecuteNonQuery();
                    Comm2.Dispose();
                    tran.Commit();
                    cnn.Close();
                    Comm2.Parameters.Clear();
                }
                catch (SqlException ex)
                {
                    
                    tran.Rollback();
                    cnn.Close();

                }
                catch (Exception ex)
                {
                   
                    tran.Rollback();
                    cnn.Close();
                }



            }
            catch
            {

               
            }
            //MessageBox.Show("Data updated successfully", "Response", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }


        public void UpdateShopData(string cntry, string SHID, string APIDATA, string shpName, string topSale)
        {

            try
            {

                cnn.ConnectionString = CnnStr;

                try
                {
                    if (cnn.State == ConnectionState.Closed) cnn.Open();
                    tran = cnn.BeginTransaction();

                    string Qry = "Update Shop_Details Set ShopDetails_Json= '"+ APIDATA + "', Shop_Name= '"+ shpName + "' , TopSales_Json = '"+ topSale + "' where ShopID ='"+SHID+"' and Country = '"+cntry+"'";
                    SqlCommand Comm2 = new SqlCommand(Qry, cnn);
                  
                    Comm2.Transaction = tran;
                    Comm2.CommandTimeout = 0;
                    Comm2.ExecuteNonQuery();
                    Comm2.Dispose();
                    tran.Commit();
                    cnn.Close();
                    Comm2.Parameters.Clear();
                }
                catch (SqlException ex)
                {

                    tran.Rollback();
                    cnn.Close();

                }
                catch (Exception ex)
                {

                    tran.Rollback();
                    cnn.Close();
                }



            }
            catch
            {


            }
            //MessageBox.Show("Data updated successfully", "Response", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }


        public void SaveVariationStockMovement(string PostScrapName, string Country, string ProdID, string ShopID, string ModelID, int SLOT, int AllocatedStock , int AvailableStock , string STRTTIME)
        {

            try
            {
                cnn.ConnectionString = CnnStr;
                if (cnn.State == ConnectionState.Closed) cnn.Open();
                tran = cnn.BeginTransaction();

                string Qry = "Insert into FSDataVariationStockMovement(ScrapName, Country,ProductID , ShopID, ModelID,Slot,AllocatedStock,AvailableStock,UnitSold,ModifiedDate)" +
               "values (@ScrapName, @Country,@ProductID , @ShopID, @ModelID,@Slot,@AllocatedStock,@AvailableStock,@UnitSold,@ModifiedDate)";
                SqlCommand Comm2 = new SqlCommand(Qry, cnn);
                Comm2.Parameters.Add("@ScrapName", SqlDbType.NVarChar, 255).Value = PostScrapName;
                Comm2.Parameters.Add("@Country", SqlDbType.NVarChar, 255).Value = Country;
                Comm2.Parameters.Add("@ProductID", SqlDbType.NVarChar, 255).Value = ProdID;
                Comm2.Parameters.Add("@ShopID", SqlDbType.NVarChar, 255).Value = ShopID;
                Comm2.Parameters.Add("@ModelID", SqlDbType.BigInt).Value = Convert.ToInt64(ModelID);
                Comm2.Parameters.Add("@Slot", SqlDbType.Int).Value = SLOT;
                Comm2.Parameters.Add("@AllocatedStock", SqlDbType.Int).Value = AllocatedStock;
                Comm2.Parameters.Add("@AvailableStock", SqlDbType.Int).Value = AvailableStock;
                int UnitSold = AllocatedStock - AvailableStock;
                Comm2.Parameters.Add("@UnitSold", SqlDbType.Int).Value =UnitSold;
                Comm2.Parameters.Add("@ModifiedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(STRTTIME);


                Comm2.Transaction = tran;
                Comm2.CommandTimeout = 0;

                Comm2.ExecuteNonQuery();
                Comm2.Dispose();
                tran.Commit();
                cnn.Close();

                Comm2.Parameters.Clear();

            }
            catch (SqlException ex)
            {
                tran.Rollback();
                cnn.Close();

            }
            catch (Exception ex)
            {
                cnn.Close();

            }
        }



        public void SavePostScrap(string Srno, string PostScrapName, string LINK_SKU, string Cat_Name, string PRODUCT_NAME, string PRICE_SLASH, string PRICE_FS, string FS_UNIT_SOLD, string ShopID, string ProdID, string Country, decimal PRICE_SLASH_UPDATED, decimal PRICE_FS_UPDATED, int SLOT , string STRTTIME)
        {
                     
                    try
                    {
                        cnn.ConnectionString = CnnStr;
                        if (cnn.State == ConnectionState.Closed) cnn.Open();
                        tran = cnn.BeginTransaction();
                      
                         string  Qry = "Insert into PostScrapData(SrNo,PostScrapName, FSPrice,LatestFSUnitSold , ModifiedDate,ProductID,ShopID,LatestSlot,LatestSlotTM,Country,SLOT1_TM,FS_UNIT_SOLD1)" +
                        "values (@SrNo,@PostScrapName, @FSPrice,@LatestFSUnitSold,@ModifiedDate,@ProductID,@ShopID,@LatestSlot,@LatestSlotTM,@Country,@SLOT1_TM,@FS_UNIT_SOLD1)";
                        SqlCommand Comm2 = new SqlCommand(Qry, cnn);
                        Comm2.Parameters.Add("@SrNo", SqlDbType.Int).Value = Convert.ToInt32(Srno);
                        Comm2.Parameters.Add("@PostScrapName", SqlDbType.NVarChar, 255).Value = PostScrapName;
                        Comm2.Parameters.Add("@FSPrice", SqlDbType.NVarChar, 255).Value = PRICE_FS;
                        Comm2.Parameters.Add("@LatestFSUnitSold", SqlDbType.NVarChar, 255).Value = FS_UNIT_SOLD;
                        Comm2.Parameters.Add("@ModifiedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(STRTTIME);
                        Comm2.Parameters.Add("@ProductID", SqlDbType.NVarChar, 255).Value = ProdID;
                        Comm2.Parameters.Add("@ShopID", SqlDbType.NVarChar, 255).Value = ShopID;
                        Comm2.Parameters.Add("@LatestSlot", SqlDbType.Int).Value = Convert.ToInt32(SLOT);
                        Comm2.Parameters.Add("@LatestSlotTM", SqlDbType.DateTime).Value = Convert.ToDateTime(STRTTIME);
                        Comm2.Parameters.Add("@Country", SqlDbType.VarChar, 255).Value = Country;
                        Comm2.Parameters.Add("@SLOT1_TM", SqlDbType.DateTime).Value = Convert.ToDateTime(STRTTIME);
                        Comm2.Parameters.Add("@FS_UNIT_SOLD1", SqlDbType.NVarChar, 255).Value = FS_UNIT_SOLD;
                        Comm2.Transaction = tran;
                        Comm2.CommandTimeout = 0;

                        Comm2.ExecuteNonQuery();
                        Comm2.Dispose();
                        tran.Commit();
                        cnn.Close();

                        Comm2.Parameters.Clear();

                    }
                    catch (SqlException ex)
                    {
                        tran.Rollback();
                        cnn.Close();

                    }
                    catch (Exception ex)
                    {
                        cnn.Close();
                       
                    }
                }




        public void SavePreScrap( string Srno, string PreScrapName, string LINK_SKU, string Cat_Name, 
            string PRODUCT_NAME, Decimal PRICE_SLASH, decimal PRICE_FS, string FS_UNIT_SOLD, string ShopID,
            string ProdID, string Country, decimal PRICE_SLASH_UPDATED, decimal PRICE_FS_UPDATED, 
            string STRTTIME, string PRICE_RANGE, string CAT_NAME_1, string CAT_NAME_2, string CAT_NAME_3,
            string Seller, string SHOP_NAME, Decimal STAR, int RATING, int TOTAL_SOLD, int MONTHLY_SOLD,
            string Variation, Decimal Variation_price, string ImgUrl,
            int FS_UNIT_SOLD_UPDATED, int MONTHLY_SOLD_UPDATED, 
            int TOTAL_SOLD_UPDATED, Decimal Variation_price_UPDATED, int Variation_Bal,
            Decimal M_PRICE, Decimal MX_PRICE,  string CTIME , string VarID, string CatID)
        {
            try
            {
                if (cnn.State == ConnectionState.Closed) cnn.Open();
                tran = cnn.BeginTransaction();

               // string Qry = "Insert into PreScrapData( SR_NO,	PreScrapName,	Cat_Name,	Cat_link,	PRODUCT_NAME,	PRICE_RANGE,	PRICE_SLASH,	PRICE_FS,	LINK_SKU,	CAT_NAME_1,	CAT_NAME_2,	CAT_NAME_3,	Seller,	SHOP_NAME,	STAR,	RATING,	TOTAL_SOLD,	MONTHLY_SOLD,	Variation,	Variation_price,	ImgUrl, InsertDt,ProdID,ShopID, Country, MONTHLY_SOLD_UPDATED,TOTAL_SOLD_UPDATED,PRICE_FS_UPDATED,PRICE_SLASH_UPDATED,VARIATION_PRICE_UPDATED,VARIATION_BALANCE,Price_Min, Price_Max, Ctime, VariationID,VariationCDate ) values (@SR_NO,	@PreScrapName,	@Cat_Name,	@Cat_link,	@PRODUCT_NAME,	@PRICE_RANGE, @PRICE_SLASH,	@PRICE_FS,	@LINK_SKU,	@CAT_NAME_1,	@CAT_NAME_2,	@CAT_NAME_3, @Seller,	@SHOP_NAME,	@STAR,	@RATING,	@TOTAL_SOLD,	@MONTHLY_SOLD,	@Variation,	@Variation_price, @ImgUrl,  @InsertDt, @ProdID, @ShopID, @Country,  @MONTHLY_SOLD_UPDATED, @TOTAL_SOLD_UPDATED, @PRICE_FS_UPDATED, @PRICE_SLASH_UPDATED, @VARIATION_PRICE_UPDATED, @VARIATION_BALANCE, @Price_Min, @Price_Max, @Ctime, @VariationID,@VariationCDate)";

                string Qry = "Insert into PreScrapData( SrNo,	PreScrapName,	FSCategory,	FSCategoryLink,	ProductName,	PriceRange,	PriceSlash,	ProductLink,	Category,	Category2,	Category3,	Seller,	ShopName,	Star,	Rating,	TotalSold,	MonthlySold,	Variation,	VariationPrice,	ImageLink,	ModifiedDate,	ShopID,	ProductID,	Country, VariationStock,		PriceMin,	PriceMax,	UnixCreationTime,	ModelID, CategoryID,	CreationDate) " +
                                   "values (  @SrNo,	@PreScrapName,	@FSCategory,	@FSCategoryLink,	@ProductName,	@PriceRange,	@PriceSlash,	@ProductLink,	@Category,	@Category2,	@Category3,	@Seller, @ShopName,	@Star,	@Rating,	@TotalSold,	@MonthlySold,	@Variation,	@VariationPrice,	@ImageLink,	@ModifiedDate,	@ShopID,	@ProductID,	@Country,	@VariationStock,	@PriceMin,	@PriceMax,	@UnixCreationTime,	@ModelID, @CategoryID,	@CreationDate )";


                SqlCommand Comm2 = new SqlCommand(Qry, cnn);

                Comm2.Parameters.Add("@SrNo", SqlDbType.Int).Value =  Convert.ToInt32(Srno);
                Comm2.Parameters.Add("@PreScrapName", SqlDbType.NVarChar, 255).Value = PreScrapName;
                Comm2.Parameters.Add("@FSCategory", SqlDbType.NVarChar, 255).Value = Cat_Name;
                Comm2.Parameters.Add("@FSCategoryLink", SqlDbType.NVarChar).Value = sModel.HostName + "flash_deals?categoryId=" + sModel.CatId + "&promotionId=" + sModel.PromotionId; // Make Custom Link
                Comm2.Parameters.Add("@ProductName", SqlDbType.NVarChar, 255).Value = PRODUCT_NAME;
                Comm2.Parameters.Add("@PriceRange", SqlDbType.NVarChar, 255).Value = PRICE_RANGE;
                Comm2.Parameters.Add("@PriceSlash", SqlDbType.Decimal).Value = PRICE_SLASH;
                Comm2.Parameters.Add("@ProductLink", SqlDbType.NVarChar).Value = LINK_SKU;
                Comm2.Parameters.Add("@Category", SqlDbType.NVarChar, 255).Value = CAT_NAME_1;
                Comm2.Parameters.Add("@Category2", SqlDbType.NVarChar, 255).Value = CAT_NAME_2;
                Comm2.Parameters.Add("@Category3", SqlDbType.NVarChar, 255).Value = CAT_NAME_3;
                Comm2.Parameters.Add("@Seller", SqlDbType.NVarChar, 255).Value = Seller;
                Comm2.Parameters.Add("@ShopName", SqlDbType.NVarChar, 255).Value = SHOP_NAME;
                Comm2.Parameters.Add("@Star", SqlDbType.Decimal).Value = STAR;
                Comm2.Parameters.Add("@Rating", SqlDbType.Int).Value = RATING;
                Comm2.Parameters.Add("@TotalSold", SqlDbType.Int).Value = TOTAL_SOLD;
                Comm2.Parameters.Add("@MonthlySold", SqlDbType.Int).Value = MONTHLY_SOLD;
                Comm2.Parameters.Add("@Variation", SqlDbType.NVarChar, 255).Value =  Variation;
                Comm2.Parameters.Add("@VariationPrice", SqlDbType.Decimal).Value =  Variation_price;
                Comm2.Parameters.Add("@ImageLink", SqlDbType.NVarChar, 255).Value = ImgUrl;
                Comm2.Parameters.Add("@ModifiedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(STRTTIME);
                Comm2.Parameters.Add("@ShopID", SqlDbType.NVarChar, 255).Value = ShopID;
                Comm2.Parameters.Add("@ProductID", SqlDbType.NVarChar, 255).Value = ProdID;
                Comm2.Parameters.Add("@Country", SqlDbType.VarChar, 255).Value = Country;
                Comm2.Parameters.Add("@VariationStock", SqlDbType.Int).Value = Variation_Bal;
                Comm2.Parameters.Add("@PriceMin", SqlDbType.Decimal).Value = M_PRICE;
                Comm2.Parameters.Add("@PriceMax", SqlDbType.Decimal).Value = MX_PRICE;
                Comm2.Parameters.Add("@UnixCreationTime", SqlDbType.VarChar, 255).Value = CTIME;
                Comm2.Parameters.Add("@CategoryID", SqlDbType.BigInt).Value = Convert.ToInt64(CatID);
                Comm2.Parameters.Add("@ModelID", SqlDbType.BigInt).Value = Convert.ToInt64(VarID);
               

                DateTimeOffset offset = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(sModel.ProEndTime));
                if (Country == "Malaysia" || Country == "Philippines" || Country == "Singapore")
                    Comm2.Parameters.Add("@CreationDate", SqlDbType.DateTime).Value = offset.DateTime.ToLocalTime().AddHours(1);
                else if (Country == "Thailand" || Country == "Indonesia" || Country == "Vietnam")
                    Comm2.Parameters.Add("@CreationDate", SqlDbType.DateTime).Value = offset.DateTime.ToLocalTime();

                Comm2.Transaction = tran;

                Comm2.CommandTimeout = 0;

                Comm2.ExecuteNonQuery();
                Comm2.Dispose();
                tran.Commit();
                cnn.Close();

                Comm2.Parameters.Clear();
               

            }
            catch (SqlException ex)
            {
              
                tran.Rollback();
                cnn.Close();

            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message.ToString());
                tran.Rollback();
                cnn.Close();
            }

        }


        public void UpdatePostScrap(int SlotSd, string SlotCountry, string STRTTIME, string FS_UNIT_SOLD, string PostScrapName, string ShopID, string ProdID)
        {
            try
            {
                cnn.ConnectionString = CnnStr;
                if (cnn.State == ConnectionState.Closed) cnn.Open();
                tran = cnn.BeginTransaction();
                string Qry = "Update PostScrapData Set SLOT" + SlotSd + "_TM = '" + STRTTIME.ToString() + "', FS_UNIT_SOLD" + SlotSd + " = '" + FS_UNIT_SOLD + "' ,  LatestSlot = '" + SlotSd + "', LatestSlotTM =  '" + STRTTIME.ToString() + "'   Where PostScrapName = '" + PostScrapName.Replace("POST_", "") + "' and Country = '" + SlotCountry + "' and ProductID = '" + ProdID + "' and ShopID = '" + ShopID + "'";
                SqlCommand Comm2 = new SqlCommand(Qry, cnn);
                Comm2.Transaction = tran;
                Comm2.CommandTimeout = 0;
                Comm2.ExecuteNonQuery();
                Comm2.Dispose();
                tran.Commit();
                cnn.Close();
            }
            catch (SqlException ex)
            {
                tran.Rollback();
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
            }

        }

        //============================================ SQL DATA CHECK FUNCTION ==================================

        //  CHECK POST SCRAP DATA ( Save or Not Save)  if Alredy save then post Data will be updated otherwise data will be inserted into postScrapData Table
        public bool IsRecordExist(string PostName, string Cntry, string Shopid, string prodid)
        {
            try
            {
                string Qry;
                DataTable dt = new DataTable();
                cnn.ConnectionString = CnnStr;
                if (cnn.State == ConnectionState.Closed) cnn.Open();
                Qry = "Select * from PostScrapData where PostScrapName = '" + PostName + "' and Country = '" + Cntry + "' and ShopId ='"+Shopid+ "' and ProductID ='" + prodid+"' ";
                SqlDataAdapter Sqldbda = new SqlDataAdapter(Qry, cnn);
                Sqldbda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    cnn.Close();
                    return true;
                }
                else
                {
                    cnn.Close();
                    return false;
                }
            }
            catch
            {
                cnn.Close();
                return false;
            }
        }

        public bool IsRecordExistNull(string PostName, string Cntry, string Shopid, string prodid, int Slot)
        {
            try
            {
                string Qry;
                DataTable dt = new DataTable();
                cnn.ConnectionString = CnnStr;
                if (cnn.State == ConnectionState.Closed) cnn.Open();
                Qry = "Select * from PostScrapData where PostScrapName = '" + PostName + "' and Country = '" + Cntry + "' and ShopId ='" + Shopid + "' and ProductID ='" + prodid + "' and SLOT"+Slot+"_TM is null";
                SqlDataAdapter Sqldbda = new SqlDataAdapter(Qry, cnn);
                Sqldbda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    cnn.Close();
                    return true;
                }
                else
                {
                    cnn.Close();
                    return false;
                }
            }
            catch
            {
                cnn.Close();
                return false;
            }
        }

        public bool IsRecordExistPreScrap(string PostName, string Cntry, string Shopid, string prodid)
        {
            try
            {
                string Qry;
                DataTable dt = new DataTable();
                cnn.ConnectionString = CnnStr;
                if (cnn.State == ConnectionState.Closed) cnn.Open();
                Qry = "Select * from PreScrapData where PreScrapName = '" + PostName + "' and Country = '" + Cntry + "' and ShopId ='" + Shopid + "' and ProdId ='" + prodid + "' ";
                SqlDataAdapter Sqldbda = new SqlDataAdapter(Qry, cnn);
                Sqldbda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    cnn.Close();
                    return true;
                }
                else
                {
                    cnn.Close();
                    return false;
                }
            }
            catch
            {
                cnn.Close();
                return false;
            }
        }

        public bool IsRecordExist_ShopDATA(string Cntry, string SHID)
        {
            try
            {
                string Qry;
                DataTable dt = new DataTable();
                cnn.ConnectionString = CnnStr;
                if (cnn.State == ConnectionState.Closed) cnn.Open();
                Qry = "Select * from Shop_Details where SHOPID = '" + SHID + "' and Country = '" + Cntry + "'";
                SqlDataAdapter Sqldbda = new SqlDataAdapter(Qry, cnn);
                Sqldbda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    cnn.Close();
                    return true;
                }

                else
                {
                    cnn.Close();
                    return false;
                }

            }
            catch
            {
                cnn.Close();
                return false;
            }
        }

        public bool IsRecordExist_PriceRange(string Cntry, string SHID, string PRID)
        {
            try
            {
                string Qry;
                DataTable dt = new DataTable();
                if (cnn.State == ConnectionState.Closed) cnn.Open();
                Qry = "select Top(1)*from PreScrapData where ProductID = '" + PRID + "' and Country = '" + Cntry + "' and ShopID = '" + SHID + "' and PriceMin != PriceMax";
                SqlDataAdapter Sqldbda = new SqlDataAdapter(Qry, cnn);
                Sqldbda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    cnn.Close();
                    return true;
                }

                else
                {
                    cnn.Close();
                    return false;
                }

            }
            catch
            {
                cnn.Close();
                return false;
            }
        }

        public bool IsRecordExistFeedback(string ModelId, string Cntry, string Shopid, string prodid)
        {
            try
            {
                string Qry;
                DataTable dt = new DataTable();
                cnn.ConnectionString = CnnStr;
                if (cnn.State == ConnectionState.Closed) cnn.Open();
                Qry = "Select * from FeedbackInfo where Country= '" + Cntry + "' and  ShopID = '" + Shopid + "' and prodid='" + prodid + "' and  ModelId = '" + ModelId + "'";
                SqlDataAdapter Sqldbda = new SqlDataAdapter(Qry, cnn);
                Sqldbda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    cnn.Close();
                    return true;
                }
                else
                {
                    cnn.Close();
                    return false;
                }
            }
            catch
            {
                cnn.Close();
                return false;
            }
        }

        public bool IsRecordExistProInfo(string promid, string Cntry)
        {
            try
            {
                string Qry;
                DataTable dt = new DataTable();
                cnn.ConnectionString = CnnStr;
                if (cnn.State == ConnectionState.Closed) cnn.Open();
                Qry = "Select * from PromotionInfo where  PromotionID = '" + promid + "' and Country= '" + Cntry + "'";
                SqlDataAdapter Sqldbda = new SqlDataAdapter(Qry, cnn);
                Sqldbda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    cnn.Close();
                    return true;
                }
                else
                {
                    cnn.Close();
                    return false;
                }
            }
            catch
            {
                cnn.Close();
                return false;
            }
        }


        public bool IsRecordExistCatInfo(string promid , string Cntry, string catid)
        {
            try
            {
                string Qry;
                DataTable dt = new DataTable();
                cnn.ConnectionString = CnnStr;
                if (cnn.State == ConnectionState.Closed) cnn.Open();
                Qry = "Select * from CategoryInfo where  PromotionID = '"+promid+"' and Country= '" + Cntry + "' and   CatID = '" + catid + "'";
                SqlDataAdapter Sqldbda = new SqlDataAdapter(Qry, cnn);
                Sqldbda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    cnn.Close();
                    return true;
                }
                else
                {
                    cnn.Close();
                    return false;
                }
            }
            catch
            {
                cnn.Close();
                return false;
            }
        }


        public bool IsRecordExistFSMovement(string ScrapName,  string Cntry,  string prodid , string Shopid, string ModelId , int slot )
        {
            try
            {
                string Qry;
                DataTable dt = new DataTable();
                cnn.ConnectionString = CnnStr;
                if (cnn.State == ConnectionState.Closed) cnn.Open();
                Qry = "Select * from FSDataVariationStockMovement where Country= '" + Cntry + "' and  ShopID = '" + Shopid + "' and ProductID ='" + prodid + "' and  ModelId = '" + ModelId + "' and ScrapName= '"+ ScrapName + "' and Slot = '"+slot+"' ";
                SqlDataAdapter Sqldbda = new SqlDataAdapter(Qry, cnn);
                Sqldbda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    cnn.Close();
                    return true;
                }
                else
                {
                    cnn.Close();
                    return false;
                }
            }
            catch
            {
                cnn.Close();
                return false;
            }
        }

        public bool IsRecordExistPreScrap(string ModelId, string Cntry, string Shopid, string prodid, string PreScrap)
        {
            try
            {
                string Qry;
                DataTable dt = new DataTable();
                cnn.ConnectionString = CnnStr;
                if (cnn.State == ConnectionState.Closed) cnn.Open();
                Qry = "Select * from PreScrapData where Country= '" + Cntry + "' and  ShopID = '" + Shopid + "' and ProductID ='" + prodid + "' and  ModelId = '" + ModelId + "' and PreScrapName= '" + PreScrap + "'";
                SqlDataAdapter Sqldbda = new SqlDataAdapter(Qry, cnn);
                Sqldbda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    cnn.Close();
                    return true;
                }
                else
                {
                    cnn.Close();
                    return false;
                }
            }
            catch
            {
                cnn.Close();
                return false;
            }
        }


        //   Get Category Name thourgh Promotion ID , CategoryId and Country 
        public string GetCatName(string PromotionID, string CatID, string Cntry)
        {
            try
            {
                string Qry;
                string CatName="";
                DataTable dt = new DataTable();
                cnn.ConnectionString = CnnStr;
                if (cnn.State == ConnectionState.Closed) cnn.Open();
                Qry = "Select CatName from CategoryInfo where PromotionID = '" + PromotionID + "' and CatID = '"+ CatID + "' and Country= '" + Cntry + "'";
                SqlDataAdapter Sqldbda = new SqlDataAdapter(Qry, cnn);
                Sqldbda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    CatName = dt.Rows[0][0].ToString();
                    cnn.Close();
                    return CatName;
                }
                else
                {
                    cnn.Close();
                    return CatName;
                }
            }
            catch
            {
                cnn.Close();
                return "";
            }
        }


        ///  CALLING FUNCTIONS 
        ///  


        public  void Delay(int dly)
        {
            var delay = System.Threading.Tasks.Task.Delay(TimeSpan.FromMilliseconds(dly));
            var seconds = 0;

            while (!delay.IsCompleted)
            {
                seconds++;
                Thread.Sleep(TimeSpan.FromMilliseconds(1000));
            }

        }



    }
}
