using System;

namespace Cadastro.Models
{
    public class Form
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DataNasc { get; set; }
        public bool Inativo { get; set; }
        public int Nacionalidade { get; set; }
        public string RG { get; set; }
    }
}
