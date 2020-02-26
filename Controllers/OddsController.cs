using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FootballAPI.Models;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace FootballAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class OddsController : ControllerBase
    {

        [HttpGet]
        public ActionResult<IEnumerable<BetData>> GetOdds_SoccerAllFormAPI()
        {
            string LeagueName;
            Event Eventdata;
            Odds Oddsdata;
            List<BetData> BetData = new List<BetData>();

            string URL = "https://app.oddsapi.io/api/v1/odds?sport=soccer&apikey=155cd660-57a7-11ea-9697-a77f73d8dc3e";
            // string URL = "https://app.oddsapi.io/api/v1/odds?sport=soccer&league=soccer-europe-champions-league&apikey=155cd660-57a7-11ea-9697-a77f73d8dc3e";
            string JsonStr = SentRequestAPI(URL);

            //แปลง Json ให้พร้อมใช้งาน
            JsonStr = JsonStr.Replace("1x2", "onextwo").Replace("\"", "\\");
            JsonStr = JsonStr.Replace(@"\1\", @"\one\").Replace(@"\2\", @"\two\").Replace("\\", "\"");
            JsonStr = JsonStr.Replace(@"888sport", @"eeesport");
            JsonStr = JsonStr.Replace(@"1xbet", @"onexbet");

            //Convert Json to object
            var ObjectOdds = JsonConvert.DeserializeObject<List<RootObject>>(JsonStr);

            int countAll = 0;
            int countIn = 0;
            foreach (var eachobject in ObjectOdds)
            {
                LeagueName = "";
                Eventdata = null;
                Oddsdata = null;
                countAll++;
                if (eachobject.Sites.onextwo != null) //เลือกเฉพาะตัวที่มีราคาต่อรองแล้ว
                {
                    countIn++;
                    //IF have League
                    if (eachobject.League.name != null)
                    {
                        LeagueName = eachobject.League.name;
                    }

                    //IF have Event
                    if (eachobject.Event != null)
                    {
                        Eventdata = eachobject.Event;
                    }

                    //IF have Odds
                    if (eachobject.Sites.onextwo != null)
                    {
                        if (eachobject.Sites.onextwo.bet365 != null)
                        {
                            Oddsdata = eachobject.Sites.onextwo.bet365.odds;
                        }
                        else if (eachobject.Sites.onextwo.sbobet != null)
                        {
                            Oddsdata = eachobject.Sites.onextwo.sbobet.odds;
                        }
                        else if (eachobject.Sites.onextwo.eeesport != null)
                        {
                            Oddsdata = eachobject.Sites.onextwo.eeesport.odds;
                        }
                        else if (eachobject.Sites.onextwo.betfair != null)
                        {
                            Oddsdata = eachobject.Sites.onextwo.betfair.odds;
                        }
                        else if (eachobject.Sites.onextwo.interwetten != null)
                        {
                            Oddsdata = eachobject.Sites.onextwo.interwetten.odds;
                        }
                        else if (eachobject.Sites.onextwo.onexbet != null)
                        {
                            Oddsdata = eachobject.Sites.onextwo.onexbet.odds;
                        }
                    }

                    var item = new BetData
                    {
                        League = LeagueName,
                        Event = Eventdata,
                        odds = Oddsdata
                    };
                    BetData.Add(item);
                }
            }
            Console.WriteLine("Count ALL : " + countAll);
            Console.WriteLine("Count IN : " + countIn);
            return BetData;
        }

        [HttpGet]
        private string SentRequestAPI(string URL)
        {
            // Create request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);

            // GET request
            request.Method = "GET";
            request.ReadWriteTimeout = 5000;
            request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream);

            // Return content
            string retString = myStreamReader.ReadToEnd();
            return retString;
        }



        // [HttpGet("{str}")]
        // public ActionResult<IEnumerable<BetData>> TestGetSoccerAllFormText(string str)
        // {
        //     string JsonStr = str;
        //     // string URL = "https://app.oddsapi.io/api/v1/odds?sport=soccer&apikey=155cd660-57a7-11ea-9697-a77f73d8dc3e";
        //     // JsonStr = SentRequestAPI(URL);

        //     string LeagueName;
        //     Event Eventdata;
        //     Odds Oddsdata;
        //     List<BetData> BetData = new List<BetData>();

        //     //แปลง Json ให้พร้อมใช้งาน
        //     JsonStr = JsonStr.Replace("1x2", "onextwo").Replace("\"", "\\");
        //     JsonStr = JsonStr.Replace(@"\1\", @"\one\").Replace(@"\2\", @"\two\").Replace("\\", "\"");

        //     //Convert Json to object
        //     var ObjectOdds = JsonConvert.DeserializeObject<List<RootObject>>(JsonStr);

        //     foreach (var eachobject in ObjectOdds)
        //     {
        //         LeagueName = "";
        //         Eventdata = null;
        //         Oddsdata = null;

        //         if (eachobject.Sites.onextwo != null) //เลือกเฉพาะตัวที่มีราคาต่อรองแล้ว
        //         {
        //             //IF have League
        //             if (eachobject.League.name != null)
        //             {
        //                 LeagueName = eachobject.League.name;
        //             }

        //             //IF have Event
        //             if (eachobject.Event != null)
        //             {
        //                 Eventdata = eachobject.Event;
        //             }

        //             //IF have Odds
        //             if (eachobject.Sites.onextwo != null)
        //             {
        //                 if (eachobject.Sites.onextwo.bet365 != null)
        //                 {
        //                     Oddsdata = eachobject.Sites.onextwo.bet365.odds;
        //                 }
        //                 else if (eachobject.Sites.onextwo.sbobet != null)
        //                 {
        //                     Oddsdata = eachobject.Sites.onextwo.sbobet.odds;
        //                 }
        //                 else if (eachobject.Sites.onextwo.eeesport != null)
        //                 {
        //                     Oddsdata = eachobject.Sites.onextwo.eeesport.odds;
        //                 }
        //                 else if (eachobject.Sites.onextwo.betfair != null)
        //                 {
        //                     Oddsdata = eachobject.Sites.onextwo.betfair.odds;
        //                 }
        //                 else if (eachobject.Sites.onextwo.interwetten != null)
        //                 {
        //                     Oddsdata = eachobject.Sites.onextwo.interwetten.odds;
        //                 }
        //             }

        //             var item = new BetData
        //             {
        //                 League = LeagueName,
        //                 Event = Eventdata,
        //                 odds = Oddsdata
        //             };
        //             BetData.Add(item);
        //         }
        //     }

        //     //convert to object
        //     return BetData;
        // }
    }
}