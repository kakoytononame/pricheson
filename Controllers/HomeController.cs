using Microsoft.AspNetCore.Mvc;
using pricheson.Models;
using System.Diagnostics;


namespace pricheson.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public Applicationdb _applicationdb=new Applicationdb();
        public Autorization _autorization=new Autorization();
        public Sendingfreetime _dayswithfreetime=new Sendingfreetime();
        public Supplier supplier = new Supplier();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            
            return View();
        }
        public IActionResult Login()
        {

            return View();
        }
        [HttpGet]
        public IActionResult Privacy(string? name,string? date)
        {

            string coc = ControllerContext.HttpContext.Request.Cookies["name"];

            if (coc != "T148823567832S")
            {
                return StatusCode(404);
            }
            if (string.IsNullOrWhiteSpace(date))
            {

                supplier.timetable = _applicationdb.GetTable();
                //supplier.twomassiv = dateandTime.dateandtimesending();
            }
            else
            {
                supplier.timetable = _applicationdb.DateandMaasterFilter(name,date);
                
                //supplier.twomassiv = dateandTime.dateandtimesending();
            }



            return View(supplier);
        }
        [HttpPost]
        public void Update(string id)
        {
            string[] deserialazejson = deserealizator(id);
            _applicationdb.dbUpdate(deserialazejson);
        }
        public void Add(string id)
        {
            string[] deserialazejson = deserealizator(id);
            for (int i = 0; i < deserialazejson.Length; i++)
            {
                Console.WriteLine(deserialazejson[i]);
            }
            _applicationdb.dbADD(deserialazejson);
        }
        public void Delete(string id)
        {
            id = id.Replace("\"", "");
            id = id.Replace("[", "");
            id = id.Replace("]", "");
            _applicationdb.dbDelete(id);
        }
        public void PasswordCheck(string id)
        {
            string[] deserialazejson= deserealizator(id);
            if (deserialazejson[0] == "саша" && deserialazejson[1] == "123")
            {
                ControllerContext.HttpContext.Response.Cookies.Append("name", "T148823567832S");
                supplier.autorization = _autorization.Checkistrue();
                Privacy("саша", "2022-08-19");
            }
            else
            {
                supplier.autorization = _autorization.Checkisfalse();
            }

        }
        public void AddZapis(string id)
        {
            //Console.WriteLine(id);
            string[] deserialazejson = deserealizator(id);
            string[] bd = new string[7];
            if(Convert.ToInt32(deserialazejson[1]) < 10)
            {
                deserialazejson[1] = "0" + deserialazejson[1] + ":00:00";
            }
            else
            {
                deserialazejson[1] = deserialazejson[1] + ":00:00";
            }
            
            deserialazejson[3] = Convert.ToDateTime(deserialazejson[3]).ToString("yyyy-MM-dd");
            for (int i = 0; i < deserialazejson.Length; i++)
            {
                Console.WriteLine(deserialazejson[i]);
            }
            bd[0]=deserialazejson[0];
            bd[1] = deserialazejson[3];
            bd[2]=deserialazejson[2];
            bd[3] = deserialazejson[6];
            bd[4] = deserialazejson[1];
            bd[5]=deserialazejson[4];
            bd[6] = deserialazejson[5];
            _applicationdb.dbzapisADD(bd);

        }
        public Sendingfreetime FindTimeforZapisi(string id)
        {
            string[] deserialazejson = deserealizator(id);
            string[] week = new string[2];
            string[] daysofweek=new string[7];
            for (int i = 0; i < 7; i++)
            {
                daysofweek[i] = DateTime.Today.AddDays(i).ToString("dd.MM.yyyy");
                //daysofweek[i].Replace(" ", "");
            }
            if (deserialazejson[1] == "1")
            {
                week[0] = DateTime.Today.ToShortDateString();
                week[1] = DateTime.Today.AddDays(7).ToShortDateString();
            };
            supplier.dateandtime = _applicationdb.DateandTimeGetFromTable(week, deserialazejson[0]);
            var massivofzapisi = supplier.dateandtime;
            List<string> list = FreeTimeFinder(Convert.ToInt32(deserialazejson[2]), massivofzapisi);
            supplier.dateandtimeforsorting = new List<DateAndTimeForSorting>();
            var bufer=supplier.dateandtimeforsorting;
            if(list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    //string[] differentparts = list[i].Split(',');
                    bufer.Add(new DateAndTimeForSorting()
                    {
                        DateAndTime = DateTime.ParseExact(list[i], "yyyy-MM-dd HH:mm:ss",
                                            System.Globalization.CultureInfo.InvariantCulture)

                    });
                }
            }
            else
            {
                supplier.sendingfreetime = _dayswithfreetime;
                return supplier.sendingfreetime;
            }
            
            bufer=bufer.OrderBy(p => p.DateAndTime).ToList();
            int m = 0;
            List<List<string>> day = new List<List<string>>();
            List<string> buferforstrings = new List<string>();
            for (int i = 0; i < bufer.Count; i++)
            {
                
                if (bufer[i].DateAndTime.ToString().Remove(bufer[i].DateAndTime.ToString().IndexOf(" ")) == daysofweek[m])
                {

                    buferforstrings.Add(bufer[i].DateAndTime.ToString());
                }
                else
                {

                    day.Add(buferforstrings);
                    if (m < daysofweek.Length)
                    {
                        m++;
                        i--;
                        buferforstrings = new List<string>();
                    }
                    
                }
            }
            
            day.Add(buferforstrings);
            for(int i=0; i< day.Count; i++)
            {
                var element=day[i];
                day[i]= element.GroupBy(x => x).Select(x => x.FirstOrDefault()).ToList();
            }
            if(day.Count == 1)
            {
                _dayswithfreetime.day1 = day[0];
            }
            if(day.Count == 2)
            {
                _dayswithfreetime.day1 = day[0];
                _dayswithfreetime.day2 = day[1];
            }
            if (day.Count == 3)
            {
                _dayswithfreetime.day1 = day[0];
                _dayswithfreetime.day2 = day[1];
                _dayswithfreetime.day3 = day[2];
            }
            if (day.Count == 4)
            {
                _dayswithfreetime.day1 = day[0];
                _dayswithfreetime.day2 = day[1];
                _dayswithfreetime.day3 = day[2];
                _dayswithfreetime.day4 = day[3];
            }
            if (day.Count == 5)
            {
                _dayswithfreetime.day1 = day[0];
                _dayswithfreetime.day2 = day[1];
                _dayswithfreetime.day3 = day[2];
                _dayswithfreetime.day4 = day[3];
                _dayswithfreetime.day5 = day[4];
            }
            if (day.Count == 6)
            {
                _dayswithfreetime.day1 = day[0];
                _dayswithfreetime.day2 = day[1];
                _dayswithfreetime.day3 = day[2];
                _dayswithfreetime.day4 = day[3];
                _dayswithfreetime.day5 = day[4];
                _dayswithfreetime.day6 = day[5];
            }
            if (day.Count == 7)
            {
                _dayswithfreetime.day1 = day[0];
                _dayswithfreetime.day2 = day[1];
                _dayswithfreetime.day3 = day[2];
                _dayswithfreetime.day4 = day[3];
                _dayswithfreetime.day5 = day[4];
                _dayswithfreetime.day6 = day[5];
                _dayswithfreetime.day7 = day[6];
            }
            if(_dayswithfreetime.day1.Count==0)
            {
                _dayswithfreetime.day1 = null;
            }
            if (_dayswithfreetime.day2.Count == 0)
            {
                _dayswithfreetime.day2 = null;
            }
            if (_dayswithfreetime.day3.Count == 0)
            {
                _dayswithfreetime.day3 = null;
            }
            if (_dayswithfreetime.day4.Count == 0)
            {
                _dayswithfreetime.day4 = null;
            }
            if (_dayswithfreetime.day5.Count == 0)
            {
                _dayswithfreetime.day5 = null;
            }
            if (_dayswithfreetime.day6.Count == 0)
            {
                _dayswithfreetime.day6 = null;
            }
            if (_dayswithfreetime.day7.Count == 0)
            {
                _dayswithfreetime.day7 = null;
            }








            supplier.sendingfreetime = _dayswithfreetime;
            return supplier.sendingfreetime;
        }
        private List<string> FreeTimeFinder(int timeforservise,List<DateandTime> list)
        {
                
                List<string> have = new List<string>();
                int differentoftime;
                int endoffirstservice;
                int beginofday = 10;
                int endofday = 20;
                string[] week = new string[7];
                string[,] alldays = new string[7, 10];
                List<string> emptydays=new List<string>();
                List<string> notemptydays=new List<string>();
                for (int i = 0; i < 7; i++)
                {
                    week[i] = DateTime.Today.AddDays(i).ToString("yyyy-MM-dd");
                    week[i].Replace(" ", "d");
                    emptydays.Add(week[i]);
                };
                for(int j = 0; j < 7; j++)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].Date.Contains(week[j]))
                        {
                            notemptydays.Add(week[j]);
                        
                        };
                    };
                };  
                notemptydays= notemptydays.GroupBy(x => x).Select(x => x.FirstOrDefault()).ToList();
                for(int i = 0; i < notemptydays.Count; i++)
                {
                    emptydays.Remove(notemptydays[i]);
                }
                if (emptydays.Count != 0) {
                    for(int i = 0; i < emptydays.Count; i++)
                    {
                        for(int j = 10; j < 21-timeforservise; j++)
                        {
                            have.Add($"{emptydays[i]}" + " " + $"{j}" + ":00:00");
                        };
                    };
                };

                if (list.Count == 1)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        endoffirstservice = Convert.ToInt32(list[i].Time.Remove(list[i].Time.IndexOf(":"))) + Convert.ToInt32(list[i].Servicedelation.Remove(list[i].Servicedelation.IndexOf(":")));
                        if (endoffirstservice < endofday)
                        {
                            differentoftime = Convert.ToInt32(endofday - endoffirstservice);
                            if (differentoftime >= timeforservise)
                            {
                                have.Add($"{list[i].Date}" + " " + $"{endoffirstservice}" + ":00:00");
                            };

                        };

                    };
                }
                for (int i = 0; i < list.Count - 1; i++)
                {
                    endoffirstservice = Convert.ToInt32(list[i].Time.Remove(list[i].Time.IndexOf(":"))) + Convert.ToInt32(list[i].Servicedelation.Remove(list[i].Servicedelation.IndexOf(":")));
                    if (endoffirstservice < endofday)
                    {
                        differentoftime = Convert.ToInt32(list[i + 1].Time.Remove(list[i].Time.IndexOf(":"))) - endoffirstservice;
                        if (differentoftime >= timeforservise)
                        {
                            have.Add($"{list[i].Date}" + " " + $"{endoffirstservice}" + ":00:00");
                        };

                    };

                };
            if (list.Count != 0)
            {
                int k = 0;
                int h = 0;
                int i = 0;
                while (i < list.Count + 1)
                {


                    
                    if (i > list.Count - 1)
                    {
                        //h--;
                        alldays[k, h] = Convert.ToInt32(list[i - 1].Time.Remove(list[i - 1].Time.IndexOf(":"))) + Convert.ToInt32(list[i - 1].Servicedelation.Remove(list[i - 1].Servicedelation.IndexOf(":"))) + ":00:00";
                        break;
                    }
                    if (list[i].Date == week[k])
                    {
                        alldays[k, h] = list[i].Time;
                        h++;
                    }
                    else
                    {
                        for(int j = 0; j < 10; j++)
                        {
                            if (alldays[k, j] is null && j==1)
                            {
                                alldays[k, h] = list[i].Time;
                                h++;
                            }
                        }
                        if (h > -1)
                        {
                            h--;
                            i--;
                            try
                            {
                                alldays[k, h] = Convert.ToInt32(list[i].Time.Remove(list[i].Time.IndexOf(":"))) + Convert.ToInt32(list[i].Servicedelation.Remove(list[i].Servicedelation.IndexOf(":"))) + ":00:00";
                            }
                            catch
                            {
                                i++;
                                alldays[k, h] = Convert.ToInt32(list[i].Time.Remove(list[i].Time.IndexOf(":"))) + Convert.ToInt32(list[i].Servicedelation.Remove(list[i].Servicedelation.IndexOf(":"))) + ":00:00";
                            }
                            
                            k++;



                        }
                        h = 0;
                    }
                    i++;
                    //if (i > list.Count - 1)
                    //{
                    //    break;
                    //}
                }
            }

                    


                
                
                for (int i = 0; i < 7; i++)
                {

                    if (!string.IsNullOrWhiteSpace(alldays[i, 0]))
                    {
                        int bufertimeforminus = Convert.ToInt32(alldays[i, 0].Remove(alldays[i, 0].IndexOf(":"))) - timeforservise;

                        for (int j = 0; j < 10; j++)
                        {
                            if (bufertimeforminus >= beginofday)
                            {


                                if (!have.Contains(Convert.ToString(bufertimeforminus) + ":00:00"))
                                {
                                    have.Add($"{week[i]}" + " " + $"{bufertimeforminus}" + ":00:00");

                                }
                                bufertimeforminus -= 1;
                            }
                        }
                    }

                }
                int indexoflastelement = 0;
                for (int i = 0; i < 7; i++)
                {

                        for (int m = 0; m < 10; m++)
                        {
                            if (string.IsNullOrWhiteSpace(alldays[i, m]))
                            {
                                indexoflastelement = m - 1;
                                m = 10;
                            }
                        }
                        if (indexoflastelement < 0)
                        {
                            indexoflastelement++;
                        }
                        if (!string.IsNullOrWhiteSpace(alldays[i, indexoflastelement]))
                        {
                            int bufertimeforplus = Convert.ToInt32(alldays[i, indexoflastelement].Remove(alldays[i, indexoflastelement].IndexOf(":")));

                            for(int j = 0; j < 10; j++)
                            {
                                if (bufertimeforplus-1 < endofday-timeforservise)
                                {
                            
                            
                                    if (!have.Contains($"{week[i]}" + " " + Convert.ToString(bufertimeforplus) + ":00:00"))
                                    {
                                        have.Add($"{week[i]}" + " " + $"{bufertimeforplus}" + ":00:00");
                            
                                    }
                                    bufertimeforplus += 1;
                                }
                            }
                            
                            
                        }
                    
                    

                }
                
                
                
                if(have is null)
                {
                    return new List<string>();
                }
                return have;
        
            

        }
        [HttpPost]
        public IActionResult ParticalViewForZapisi(string id)
        {
            
            return PartialView(FindTimeforZapisi(id));
        }
        public IActionResult ParticalViewForFIOAndPhone()
        {

            return PartialView();
        }
        public string[] deserealizator(string id)
        {
            id = id.Replace("\"", "");
            id = id.Replace("[", "");
            id = id.Replace("]", "");
            string[] deserialazejson = id.Split(",");
            return deserialazejson;
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}