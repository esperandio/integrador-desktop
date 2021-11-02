using Microsoft.Data.Sqlite;
using MonitoramentoTempoOcioso.Entities.Events;
using MonitoramentoTempoOcioso.Factories;
using MonitoramentoTempoOcioso.Interfaces.Events;
using System;
using System.Collections.Generic;

namespace MonitoramentoTempoOcioso.Repositories.Events
{
    class SQLiteEventRepository : IEventRepository
    {
        private readonly SqliteConnection _sqliteConnection;

        public SQLiteEventRepository()
        {
            string pathDataSourceFile = Environment.ExpandEnvironmentVariables(@"%appdata%\events.db");

            var connectionString = new SqliteConnectionStringBuilder()
            {
                DataSource = pathDataSourceFile,
                Mode = SqliteOpenMode.ReadWriteCreate
            }.ToString();

            _sqliteConnection = new SqliteConnection(connectionString);

            _sqliteConnection.Open();

            using var command = _sqliteConnection.CreateCommand();

            command.CommandText = @"
                 CREATE TABLE IF NOT EXISTS events (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ds_event TEXT,
                    dt_sync TEXT
                 );
            ";

            command.ExecuteNonQuery();
        }

        public void Add(IEvent @event)
        {
            using var command = _sqliteConnection.CreateCommand();

            command.CommandText = @"
                INSERT INTO events
                    (ds_event) 
                VALUES
                    (@ds_event);
            ";

            command.Parameters.AddWithValue("@ds_event", @event.SerializeObject());

            command.ExecuteNonQuery();
        }

        public int Count()
        {
            using var command = _sqliteConnection.CreateCommand();

            command.CommandText = @"
                SELECT 
                    COUNT(*) AS nr_count 
                FROM 
                    events;
            ";

            using var reader = command.ExecuteReader();

            reader.Read();
            return reader.GetInt32(0);
        }

        public List<IEvent> GetEventsToSync()
        {
            using var command = _sqliteConnection.CreateCommand();

            command.CommandText = @"
                SELECT 
                    ds_event
                FROM 
                    events
                WHERE
                    dt_sync IS NULL;
            ";

            using var reader = command.ExecuteReader();

            List<IEvent> eventsToSync = new List<IEvent>();

            while (reader.Read())
            {
                EventDTO eventDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<EventDTO>(reader.GetString(0));
                IEvent @event = EventFactory.Create(eventDTO);

                eventsToSync.Add(@event);
            }

            return eventsToSync;
        }
    }
}
