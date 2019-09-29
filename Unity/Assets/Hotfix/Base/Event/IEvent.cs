﻿using System;

namespace ETHotfix
{
#if ILRuntime
 public interface IEvent
    {
        void Handle();
        void Handle(object a);
        void Handle(object a, object b);
        void Handle(object a, object b, object c);
        void Handle(object a, object b, object c, object d);
    }

    public abstract class AEvent: IEvent
    {
        public void Handle()
        {
            this.Run();
        }

        public void Handle(object a)
        {
            throw new NotImplementedException();
        }

        public void Handle(object a, object b)
        {
            throw new NotImplementedException();
        }

        public void Handle(object a, object b, object c)
        {
            throw new NotImplementedException();
        }

        public void Handle(object a, object b, object c, object d)
        {
            throw new NotImplementedException();
        }

        public abstract void Run();
    }

    public abstract class AEvent<A>: IEvent
    {
        public void Handle()
        {
            throw new NotImplementedException();
        }

        public void Handle(object a)
        {
            this.Run((A) a);
        }

        public void Handle(object a, object b)
        {
            throw new NotImplementedException();
        }

        public void Handle(object a, object b, object c)
        {
            throw new NotImplementedException();
        }

        public void Handle(object a, object b, object c, object d)
        {
            throw new NotImplementedException();
        }

        public abstract void Run(A a);
    }

    public abstract class AEvent<A, B>: IEvent
    {
        public void Handle()
        {
            throw new NotImplementedException();
        }

        public void Handle(object a)
        {
            throw new NotImplementedException();
        }

        public void Handle(object a, object b)
        {
            this.Run((A) a, (B) b);
        }

        public void Handle(object a, object b, object c)
        {
            throw new NotImplementedException();
        }

        public void Handle(object a, object b, object c, object d)
        {
            throw new NotImplementedException();
        }

        public abstract void Run(A a, B b);
    }

    public abstract class AEvent<A, B, C>: IEvent
    {
        public void Handle()
        {
            throw new NotImplementedException();
        }

        public void Handle(object a)
        {
            throw new NotImplementedException();
        }

        public void Handle(object a, object b)
        {
            throw new NotImplementedException();
        }

        public void Handle(object a, object b, object c)
        {
            this.Run((A) a, (B) b, (C) c);
        }

        public void Handle(object a, object b, object c, object d)
        {
            throw new NotImplementedException();
        }

        public abstract void Run(A a, B b, C c);
    }

    public abstract class AEvent<A, B, C, D>: IEvent
    {
        public void Handle()
        {
            throw new NotImplementedException();
        }

        public void Handle(object a)
        {
            throw new NotImplementedException();
        }

        public void Handle(object a, object b)
        {
            throw new NotImplementedException();
        }

        public void Handle(object a, object b, object c)
        {
            throw new NotImplementedException();
        }

        public void Handle(object a, object b, object c, object d)
        {
            this.Run((A) a, (B) b, (C) c, (D) d);
        }

        public abstract void Run(A a, B b, C c,D d);
    }
#else
    public abstract class AEvent: ETModel.AEvent
    {
    }

    public abstract class AEvent<A>: ETModel.AEvent<A>
    {
    }

    public abstract class AEvent<A, B>: ETModel.AEvent<A, B>
    {
    }

    public abstract class AEvent<A, B, C>: ETModel.AEvent<A, B, C>
    {
    }

    public abstract class AEvent<A, B, C, D>: ETModel.AEvent<A, B, C, D>
    {
    }
#endif
}