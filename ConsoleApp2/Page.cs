using Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;

namespace ConsoleApp2
{
    public class Page
    {
        public string PageName { get; set; }
        public List<Page> pageList = new List<Page>();
        public List<Service> serviceList = new List<Service>();
        public Service services;
        public OutResponse<List<ServicesDTO>> res = new OutResponse<List<ServicesDTO>>();
        public Page()
        {
        }

        public virtual void DisplayMenu()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/branch.txt";
            string BranchName = "";
            int BranchID;
            string val = "";
            if (File.Exists(path))
            {
                val = File.ReadAllText(path);
            }
            if (val != "" || val != null)
            {
                BranchID = Convert.ToInt32(val);
                using (HttpClient client = new HttpClient())
                {
                    res = client.GetFromJsonAsync<OutResponse<List<ServicesDTO>>>($"https://localhost:44391/api/Service/{BranchID}").Result;

                    if(res.ResData.Count == 0)
                    {
                        Console.WriteLine("No Services Available");
                        Console.ReadLine();
                        Environment.Exit(0);
                    }else
                    {
                        foreach (var item in res.ResData)
                        {
                            services = new Service
                            {
                                BranchID = BranchID,
                                ServiceID = item.ServiceID,
                                ServiceName = item.ServiceName
                            };
                            serviceList.Add(services);
                            BranchName = item.BranchName;
                        }
                        Console.WriteLine($"Welcome To {BranchName} Branch");
                    }
                }
                foreach (var item in serviceList)
                {
                    Console.WriteLine(serviceList.IndexOf(item) + 1 + " -- " + item.ServiceName);
                }
                int choice = GetReader.GetInteger("Enter correct choice");

                if(choice > serviceList.Count)
                {
                    this.DisplayMenu();
                }
                Console.Clear();
                serviceList[choice - 1].DisplayQuestions();

            }else
            {
                Console.WriteLine("Check File Content");
            }
            Console.ReadLine();
            Console.Clear();
        }

    }
}