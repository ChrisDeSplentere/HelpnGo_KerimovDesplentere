using HelpnGo.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HelpnGo.DAL
{
    class DataAccess
    {

        public DataAccess()
        {

        }

        public async Task<ObservableCollection<City>> getCitiesAsync()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://helpngowebapi.azurewebsites.net/api/cities/all");
            string json = await response.Content.ReadAsStringAsync();
            var cities = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<City>>(json);

            return cities;

        }

        public async Task<ObservableCollection<string>> getCategoriesAsync()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://helpngowebapi.azurewebsites.net/api/categories/all");
            string json = await response.Content.ReadAsStringAsync();
            var categories = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Category>>(json);
            ObservableCollection<string> catNames = new ObservableCollection<string>();
            foreach (Category cat in categories)
            {
                catNames.Add(cat.Label);
            }
            return catNames;
        }

        public async Task<ObservableCollection<string>> getProvincesAsync()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://helpngowebapi.azurewebsites.net/api/provinces/all");
            string json = await response.Content.ReadAsStringAsync();
            var provinces = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Province>>(json);
            ObservableCollection<string> provNames = new ObservableCollection<string>();
            foreach (Province prov in provinces)
            {
                provNames.Add(prov.Label);
            }
            return provNames;
        }

        public async Task<ObservableCollection<Publication>> getPublicationsAsync(bool offersChecked, bool demandsChecked, string category, string province)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://helpngowebapi.azurewebsites.net/api/publications/all");
            string json = await response.Content.ReadAsStringAsync();
            var allPublications = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Publication>>(json);

            ObservableCollection<Publication> selectedPublications = new ObservableCollection<Publication>();
            foreach (Publication pub in allPublications)
            {
                if ((offersChecked && pub.Is_offer) || (demandsChecked && !pub.Is_offer))
                {
                    if (category == "(All)" || category == pub.Category_label)
                    {
                        if (province == "(All)" || province == pub.Province_label)
                        {
                            selectedPublications.Add(pub);
                        }
                    }
                }
            }

            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            foreach (Publication pub in selectedPublications)
            {
                if (pub.Is_offer)
                {
                    pub.Type = loader.GetString("offer");
                }
                else
                {
                    pub.Type = loader.GetString("demand");
                }
            }
            
            return selectedPublications;
        }

        public async Task<User> getUserByEmail(string email)
        {
            
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://helpngowebapi.azurewebsites.net/api/users/byEmail/?email=" + email);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                var user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(json);
                return user;
            }
            else
            {
                return null;
            }
               
        }

        public async Task<decimal> getNewPublicationID()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://helpngowebapi.azurewebsites.net/api/publications/newId");
            string json = await response.Content.ReadAsStringAsync();
            decimal newIdPub = Convert.ToDecimal(json, CultureInfo.InvariantCulture);
            return newIdPub;

        }

        public async Task<decimal> getNewUserId()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://helpngowebapi.azurewebsites.net/api/users/newId");
            string json = await response.Content.ReadAsStringAsync();
            decimal newIdUser = Convert.ToDecimal(json, CultureInfo.InvariantCulture);
            return newIdUser;
        }

        public async Task<ObservableCollection<Publication>> getUsersPublicationsAsync(decimal userId)
        {
            HttpClient client = new HttpClient();
            string sUserId = Convert.ToString(userId, CultureInfo.InvariantCulture);
            HttpResponseMessage response = await client.GetAsync("http://helpngowebapi.azurewebsites.net/api/publications/byUserId/?userId=" + sUserId);
            string json = await response.Content.ReadAsStringAsync();
            ObservableCollection<Publication> selectedPublications = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Publication>>(json);


            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            foreach (Publication pub in selectedPublications)
            {
                if (pub.Is_offer)
                {
                    pub.Type = loader.GetString("offer");
                }
                else
                {
                    pub.Type = loader.GetString("demand");
                }
            }


            return selectedPublications;

        }

        public async Task<bool> setPublicationAsync(Publication pub)
        {
            HttpClient client = new HttpClient();

            string isOffer = (pub.Is_offer) ? "True" : "False";

            var values = new Dictionary<string, string>
            {
               { "Publication_id", Convert.ToString(pub.Publication_id, CultureInfo.InvariantCulture) },
               { "Author_id", Convert.ToString(pub.Author_id, CultureInfo.InvariantCulture) },
               { "Category_label", pub.Category_label },
               { "Description", pub.Description },
               { "Email", pub.Email },
               { "Phone_number", pub.Phone_number },
               { "Province_label", pub.Province_label },
               { "Title", pub.Title},
               { "Is_offer", isOffer }
            };

            var content = new FormUrlEncodedContent(values);

            HttpResponseMessage response = await client.PostAsync("http://helpngowebapi.azurewebsites.net/api/publications", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> updatePublicationAsync(Publication pub)
        {
            HttpClient client = new HttpClient();

            string isOffer = (pub.Is_offer) ? "True" : "False";

            var values = new Dictionary<string, string>
            {
               { "Publication_id", Convert.ToString(pub.Publication_id, CultureInfo.InvariantCulture) },
               { "Author_id", Convert.ToString(pub.Author_id, CultureInfo.InvariantCulture) },
               { "Category_label", pub.Category_label },
               { "Description", pub.Description },
               { "Email", pub.Email },
               { "Phone_number", pub.Phone_number },
               { "Province_label", pub.Province_label },
               { "Title", pub.Title},
               { "Is_offer", isOffer } 
            };

            var content = new FormUrlEncodedContent(values);

            string sPubId = Convert.ToString(pub.Publication_id, CultureInfo.InvariantCulture);
            HttpResponseMessage response = await client.PutAsync("http://helpngowebapi.azurewebsites.net/api/publications/?id=" + sPubId, content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> deletePublicationAsync(decimal pubId)
        {
            HttpClient client = new HttpClient();
            string sPubId = Convert.ToString(pubId, CultureInfo.InvariantCulture);
            HttpResponseMessage response = await client.DeleteAsync("http://helpngowebapi.azurewebsites.net/api/publications/?id=" + sPubId);
            return response.IsSuccessStatusCode;
        }



        public async Task<bool> setNewUserAsync(User user)
        {
            HttpClient client = new HttpClient();

            var values = new Dictionary<string, string>
            {

               { "User_id", Convert.ToString(user.User_id, CultureInfo.InvariantCulture) },
               { "Date_of_birth",  null},
               { "Diplomas", user.Diplomas },
               { "Email", user.Email },
               { "Firstname", user.Firstname},
               { "Jobs", user.Jobs },
               { "Lastname", user.Lastname},
               { "Phone_number", user.Phone_number },
               { "Street_name", user.Street_name},
               { "Street_number", user.Street_number },
               { "Living_city_id", Convert.ToString(user.Living_city_id, CultureInfo.InvariantCulture) },
               { "password", user.password}
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("http://helpngowebapi.azurewebsites.net/api/users", content);

            return response.IsSuccessStatusCode;
        }

    }
}
