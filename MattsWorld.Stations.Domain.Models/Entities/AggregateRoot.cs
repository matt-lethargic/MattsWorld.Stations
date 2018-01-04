using System;
using System.Collections.Generic;

namespace MattsWorld.Stations.Domain.Models.Entities
{
    public abstract class AggregateRoot
    {
        private readonly List<Event> _changes = new List<Event>();

        public abstract Guid Id { get; }

        public int Version { get; internal set; }

        public IEnumerable<Event> GetUncomittedChanges()
        {
            return _changes;
        }

        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        public void LoadFromHistory(IEnumerable<Event> history)
        {
            foreach (var e in history) ApplyChanges(e, false);
        }

        protected void ApplyChanges(Event @event)
        {
            ApplyChanges(@event, true);
        }

        private void ApplyChanges(Event @event, bool isNew)
        {
            this.AsDynamic().Apply(@event);
            if (isNew) _changes.Add(@event);
        }
    }
}
