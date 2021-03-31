using DataAccessLayer;
using ArchiveModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace ArchiveBusinessModel
{
    public class ReceiveLetterManager<T> where T:class
    {
        DatabaseContext DBC = new DatabaseContext();

        public void Add(List<T> Entities)
        {
            foreach (T Entity in Entities)
            {
                DBC.Set<T>().Add(Entity);
                DBC.Set<T>().Add(Entity);
                if (DBC.SaveChanges()==0) throw new Exception();
            }
 
        }

        public void Add(ReceivedLetter Entity)
        {
            DBC.ReceiveLetters.Add(Entity);
            if (DBC.SaveChanges() == 0) throw new Exception(Entity.Id.ToString(), new Exception());
        }

        public List<ReceivedLetter> Read()
        {
                
        }
        public ReceivedLetter Read(int Id)
        {
            return DBC.ReceiveLetters.Find(Id);
        }

        public void Remove(ReceivedLetter Entity)
        {
            if (!Equals(DBC.ReceiveLetters.Remove(DBC.ReceiveLetters.Find(Entity.Id)), Entity))
                throw new Exception(Entity.Id.ToString(), new Exception());
        }

        public void Ubdate(ReceivedLetter Entity)
        {
            DBC.Entry(Entity).State = EntityState.Modified;
            if (DBC.SaveChanges()==0)
                throw new Exception(Entity.Id.ToString(), new Exception());

        }

        public void Ubdate(List<ReceivedLetter> Entities)
        {
            foreach (ReceivedLetter Entity in Entities)
            {
                DBC.Entry(Entity).State = EntityState.Modified;
                if (DBC.SaveChanges() == 0)
                    throw new Exception(Entity.Id.ToString(), new Exception());
            }
        }       
    }
}
