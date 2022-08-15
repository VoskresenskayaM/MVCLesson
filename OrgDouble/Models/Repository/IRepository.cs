using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgDouble.Models.Repository
{
   public interface IRepository
    {
        IQueryable<Person> Persons { get; }
        void SetAllPersonBase(int firstId, int lastId);
      public  Person GetPerson(int Id);
        public void SetCheck(int Id);
        public void Сlear();
        public IQueryable<Person> SelectAll();
        public void DelitePersons(List<Person> persons);
       
    }
}
