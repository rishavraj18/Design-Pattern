﻿using System.Threading.Tasks;

namespace StructuralPatterns.Decorator {
    public abstract class NotificationDecorator : Notifier {
        protected Notifier Component;

        public NotificationDecorator(Notifier component) {
            Component = component;
        }

        public override async Task HandleTableReadyMessage() 
            => await Component.HandleTableReadyMessage();
    }
}
