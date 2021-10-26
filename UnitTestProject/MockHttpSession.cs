﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

/*
 Kode hentet fra Canvas-modul "Enhetstest av KundeApp-logginn løsning" (i faget ITPE3200-1 21H).
 */

/*
 
namespace UnitTestProject
{
    public class MockHttpSession : ISession
    {
        Dictionary<string, object> sessionStorage = new Dictionary<string, object>();

        public object this[string name]
        {
            get { return sessionStorage[name]; }
            set { sessionStorage[name] = value; }
        }

        void ISession.Set(string key, byte[] value)
        {
            sessionStorage[key] = value;
        }

        bool ISession.TryGetValue(string key, out byte[] value)
        {
            if (sessionStorage[key] != null)
            {
                value = Encoding.ASCII.GetBytes(sessionStorage[key].ToString());
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }
    }
}

*/