using System.Data.Entity;
using Model;

namespace DataLayer
{
  public static class Helpers
  {
    private static EntityState ConvertState(ObjectState objectState)
    {
      switch (objectState)
      {
        case ObjectState.Added:
          return EntityState.Added;
        case ObjectState.Modified:
          return EntityState.Modified;
        case ObjectState.Deleted:
          return EntityState.Deleted;
        default:
          return EntityState.Unchanged;
      }
    }

    public static void ApplyStateChanges(this DbContext context)
    {
      foreach (var entry in context.ChangeTracker.Entries<IObjectWithState>())
      {
        IObjectWithState stateInfo = entry.Entity;
        entry.State = ConvertState(stateInfo.ObjectState);
      }
    }
  }
}
