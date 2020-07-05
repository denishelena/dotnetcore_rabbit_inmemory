using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using projetoAlan.Models;
using projetoAlan.Context;
using projetoAlan.Rabbitmq;
using RestSharp;



namespace projetoAlan.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]


    public class ZipCode
        {
            public string cep { get; set; }
            public string logradouro { get; set; }
            public string complemento { get; set; }
            public string bairro { get; set; }
            public string localidade { get; set; }
            public string uf { get; set; }
            public string unidade { get; set; }
            public string ibge { get; set; }
            public string gia { get; set; }
        }


    public class WeatherForecastController : ControllerBase
    {
        private MyContext _context {get; set;}
        
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,MyContext Context)
        {
            _logger = logger;
            _context = Context;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            //var s = new Sender();
            //s.Send();
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("lista")]
        public String GetLista(String cep)
        {
            
            var client = new RestClient($"http://viacep.com.br/ws/{cep}/json/");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            
            return response.Content;
            
            //return _context.Usuarios.ToList();

        }

        [HttpGet("{id}")]
        public Usuario GetById([FromRoute] int id)
        {
            return _context.Usuarios.FirstOrDefault(S => S.Id == id);
        }

        [HttpPost]
        public String PostUsuario([FromBody]Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            var sender = new Sender();
            sender.Send(usuario);
            return "dados inseridos com sucesso";
        }


        [HttpDelete("{id}")]
        public bool DeleteById([FromRoute] int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(S => S.Id == id);

            if (usuario != null){
                _context.Usuarios.Remove(usuario);
                _context.SaveChanges();
                return true;
            }
        return false;
        }
    }
}                                                                           