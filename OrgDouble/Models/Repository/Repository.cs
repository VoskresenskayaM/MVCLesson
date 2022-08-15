
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OrgDouble.Models.Repository
{
    public class Repository : IRepository
    {
        private OrgDoubleContex _context;
        public Repository(OrgDoubleContex context)
        {
            _context = context;
        }
        public IQueryable<Person> Persons => _context.Persons;
        public void DelitePersons(List<Person> persons)
        {
            foreach (Person p in persons)
             _context.Persons.Remove(p);
            _context.SaveChanges();
        }
        public Person GetPerson(int Id)
        {
        
            return _context.Persons.FirstOrDefault(p => p.Id == Id);
        }
        public void SetCheck(int Id)
        {
            var person = _context.Persons.FirstOrDefault(p=>p.Id==Id);
            if (person.Done == false)
                person.Done = true;
            _context.SaveChanges();
        }
        public void Сlear()
        {
            _context.Persons.RemoveRange(_context.Persons);
            _context.SaveChanges();
        }
        public IQueryable<Person> SelectAll()
        {
            var persons = _context.Persons.FromSqlRaw(@"SELECT * FROM Persons
                                                      WHERE 
	                                                  LastName IN(SELECT LastName FROM Persons 
                                                      GROUP BY LastName 
                                                      HAVING COUNT(*) > 1)
                                                      AND
                                                      FirstName IN(SELECT FirstName FROM Persons 
                                                      GROUP BY FirstName 
                                                      HAVING COUNT(*) > 1)
                                                      AND 
                                                      SecondName IN(SELECT SecondName FROM Persons 
                                                      GROUP BY SecondName 
                                                      HAVING COUNT(*) > 1)
                                                      AND
                                                      PersonBirth IN(SELECT PersonBirth FROM Persons 
                                                      GROUP BY PersonBirth 
                                                      HAVING COUNT(*) > 1)
                                                      ORDER BY
                                                      LastName");
            return persons;
        }

        public void SetAllPersonBase(int firstId, int lastId)
        {
            int currentId = firstId;
            int index = 10; 
         
            while(firstId+1< lastId)
            {
                if (lastId - firstId < 10) index = lastId - firstId;

                SetPersonBase(currentId, index);
                currentId += 10;
                firstId +=10;
            }



            //int res = (lastId - firstId) / 10;
            //int ost = (lastId - firstId) % 10;
            //for (int k = 0; k <= res; k++)
            //{
            //    int index = 10;
            //    if (k == res)
            //    {
            //        index = ost;
            //    }
            //}

        }

        public void SetPersonBase(int currentId, int index)
        {/*доделать остаток по модулю*/
            //int currentId = firstId;
            //int res = (lastId - firstId) / 10;
            //int ost = (lastId - firstId) % 10;

            //for (int k = 0; k <= res; k++)
            //{
            //    int index = 10;
            //    if (k == res)
            //    {
            //        index = ost;
            //    }

                StringBuilder responseBodyAll = new StringBuilder();
                for (int i = 0; i < index; i++)
                {
                    var client = new HttpClient();
                    var url = $"XXXXXXXXXX";
                    var response =
                           client.GetAsync(url).Result;
                  if(response.IsSuccessStatusCode)
                {  var responseBody = response.Content.ReadAsStringAsync().Result;
            
                    if (i != index - 1) { responseBodyAll.Append(responseBody + ","); }
                    else
                        responseBodyAll.Append(responseBody);
                    /* Console.WriteLine(responseBodyAll.ToString());*/
                    currentId++;
                }
                }
                string responseBodyString = $"{{\"items\":[" + responseBodyAll.ToString() + "]}";
                Console.WriteLine(responseBodyString.Length);

                JObject jObject = JObject.Parse(responseBodyString);
                int count = jObject.Count;

            /* Persons.Add (jObject.ToObject<List<Person>>());*/
            IEnumerable<Person> l= new  List<Person>() ;
            

                 l = jObject["items"].Select(i => new  Person 
                {
            
                    LastName = i["PersonLastNameRu"].ToString(),
                    FirstName = i["PersonFirstNameRu"].ToString(),
                    SecondName = i["PersonSecondNameRu"].ToString(),
                    PersonBirth = /*DateTime.Parse((string)*/i["PersonBirth"].ToString()
                   
                });
              
      
            try
            {
                foreach (var a in l)
                {
                /* Person person = new Person { LastName = a.LastName, FirstName = a.FirstName, SecondName = a.SecondName, PersonBirth = a.PersonBirth };*/
             
                    _context.Persons.Add(a /*new Person { LastName = a.LastName, FirstName = a.FirstName, SecondName = a.SecondName, PersonBirth = a.PersonBirth }*/);


                }
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex.Message);
            }
            _context.SaveChanges();
            }
        //}


    }
}


//работает по 50


//public void SetPersonBase(int firstId, int lastId)
//        {/*доделать остаток по модулю*/
//            int currentId = firstId;
//            int res = (lastId - firstId) / 10;
//            int ost = (lastId - firstId) % 10;

//            for (int k = 0; k <= res; k++)
//            {
//                int index=10;
//                if (k == res)
//                {
//                    index = ost;
//                }    
//                    StringBuilder responseBodyAll = new StringBuilder();
//                    for (int i = 0; i < index; i++)
//                    {
//                        var client = new HttpClient();
//                        var url = $"XXXXXXX";
//                        var response =
//                               client.GetAsync(url).Result;
//                        var responseBody = response.Content.ReadAsStringAsync().Result;
//                        if (i != 9 && currentId != lastId-1) { responseBodyAll.Append(responseBody + ","); }
//                        else
//                            responseBodyAll.Append(responseBody);
//                        /* Console.WriteLine(responseBodyAll.ToString());*/
//                        currentId++;
//                    }
//                    string responseBodyString = $"{{\"items\":[" + responseBodyAll.ToString() + "]}";
//                    Console.WriteLine(responseBodyString.Length);

//                    JObject jObject = JObject.Parse(responseBodyString);
//                    int count = jObject.Count;

//                    /* Persons.Add (jObject.ToObject<List<Person>>());*/

//                    var l = jObject["items"].Select(i => new Person
//                    {
//                        LastName = i["PersonLastNameRu"].ToString(),
//                        FirstName = i["PersonFirstNameRu"].ToString(),
//                        SecondName = i["PersonSecondNameRu"].ToString(),
//                        PersonBirth = /*DateTime.Parse((string)*/i["PersonBirth"].ToString()

//                    });


//                    foreach (var a in l)
//                    {
//                        /* Person person = new Person { LastName = a.LastName, FirstName = a.FirstName, SecondName = a.SecondName, PersonBirth = a.PersonBirth };*/

//                        _context.Persons.Add(a /*new Person { LastName = a.LastName, FirstName = a.FirstName, SecondName = a.SecondName, PersonBirth = a.PersonBirth }*/);


//                    }
//                    _context.SaveChanges();
//                }
//            }

     
//    }
//}
