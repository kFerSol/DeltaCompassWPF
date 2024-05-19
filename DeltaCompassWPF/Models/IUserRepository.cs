﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DeltaCompassWPF.Models
{
    public interface IUserRepository
    {
        bool AuthenticateUser(NetworkCredential credential);
        void Add(Usuario usuario);
        void Edit(Usuario usuario);
        void remove (int id);
        Usuario GetById(int id);
        Usuario GetByUsername(string nome);
        Usuario GetInformacoesAutenticadas(string nome);
        IEnumerable<Usuario> GetByAll();
        int GetUserId(string nome);

    }
}