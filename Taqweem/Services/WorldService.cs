using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using Taqweem.Classes;
using Taqweem.Data;
using Taqweem.Models;

namespace Taqweem.Services
{
    public class WorldService
    {
        private readonly EFRepository Repository;
        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment env;
        private readonly ApplicationDbContext _context;

        public WorldService(IConfiguration Configuration,
                            IHostingEnvironment Env,
                            ApplicationDbContext context)
        {
            env = Env;
            configuration = Configuration;

            _context = context;
            Repository = new EFRepository(_context);
        }

        public bool OpenExchangeRates()
        {
            try
            {
                string BaseCurrency = "USD";
                string AppId = configuration.GetValue<string>("OpenExchange_APIKey");
                string ApiUrl = "https://openexchangerates.org/api/latest.json?app_id=" + AppId + "&base=" + BaseCurrency;

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ApiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.GetAsync(ApiUrl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync();

                        //dynamic table = data.Result.ConvertFromJson<ExpandoObject>();
                        dynamic table = OptionConfigExtentions
                                        .ConvertFromJson<ExpandoObject>(data.Result);// data.Result.ConvertFromJson<ExpandoObject>();

                        var result = new Dictionary<string, double>();
                        dynamic rates = table.rates;

                        foreach (var x in rates)
                            result.Add(x.Key, x.Value);

                        double ZARValue = result["ZAR"];

                        List<CurrencyHistory> Currencies = new List<CurrencyHistory>();
                        foreach (var item in result)
                        {
                            CurrencyHistory currency = new CurrencyHistory();
                            currency.ConversionRate = ZARValue / item.Value;
                            currency.Code = item.Key;
                            currency.DateTimeStamp = DateTime.UtcNow;
                            Currencies.Add(currency);
                        }

                        Repository.AddMultiple(Currencies);

                        //TO DO SEND EMAIL
                    }

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void UpdateCurrency(List<Currency> model)
        {
            Repository.UpdateMultiple<Currency>(model);
        }

        public Currency CurrencyGetByCode(string Code)
        {
            return Repository.Find<Currency>(c => c.Code == Code).FirstOrDefault();
        }

        public List<Currency> CurrencyGetAll()
        {
            return Repository.GetAll<Currency>().ToList();
        }
    }
}
