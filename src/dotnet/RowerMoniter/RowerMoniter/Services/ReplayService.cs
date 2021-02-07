using Newtonsoft.Json;
using RowerMoniter.Contracts;
using RowerMoniter.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RowerMoniter.Services
{
    public class ReplayService
    {
        public ReplayService() 
        {
        }

        public async Task ReplayFile(string path, CancellationToken token) 
        {
            await Task.Run(() => {
                Log log;

                using (StreamReader file = File.OpenText(path))
                {
                    var serializer = JsonSerializer.Create(new JsonSerializerSettings() { Formatting = Formatting.Indented, TypeNameHandling = TypeNameHandling.All });
                    log = (Log)serializer.Deserialize(file, typeof(Log));
                }

                while (token.IsCancellationRequested == false)
                {
                    var queue = new Queue<LoggedEvent>();

                    foreach (var message in log.Events.OfType<LoggedEvent>())
                    {
                        queue.Enqueue(message);
                    }

                    ReplayMessages(queue);
                }
            }); 
            
        }

        private void ReplayMessages(Queue<LoggedEvent> queue)
        {
            while (queue.Any()) 
            {
                var e = queue.Dequeue();
                Thread.Sleep(e.Ellapsed);
                
                EventService.Instance.ParseAndPublish(e.Message);
            }
        }
    }
}
