using Contracts;
using Entities;
using Entities.Helpers;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;


namespace Repository
{
    
    public class OwnerRepository : RepositoryBase<Owner>, IOwnerRepository
    {
        private ISortHelper<Owner> _sortHelper;
        public OwnerRepository(RepositoryContext repositoryContext, ISortHelper<Owner> sortHelper)
            : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public void CreateOwner(Owner owner)
        {
            Create(owner);
        }

        public void DeleteOwner(Owner owner)
        {
            Delete(owner);
        }

        public Owner GetOwnerById(Guid ownerId)
        {
            return FindByCondition(owner => owner.OwnerId.Equals(ownerId))
                    .FirstOrDefault();
        }

        public PagedList<Owner> GetOwners(OwnerParameters ownerParameters)
        {
            var owners = FindByCondition(o => o.DateOfBirth.Year >= ownerParameters.MinYearOfBirth &&
                                        o.DateOfBirth.Year <= ownerParameters.MaxYearOfBirth);

            SearchByName(ref owners, ownerParameters.Name);
            
            return PagedList<Owner>.ToPagedList(owners.OrderBy(on => on.Name),
                ownerParameters.PageNumber,
                ownerParameters.PageSize);
        }

        private void SearchByName(ref IQueryable<Owner> owners, string ownerName)
        {
            if (!owners.Any() || string.IsNullOrWhiteSpace(ownerName))
                return;

            owners = owners.Where(o => o.Name.ToLower().Contains(ownerName.Trim().ToLower()));
        }

       

        public Owner GetOwnerWithDetails(Guid ownerId)
        {
            return FindByCondition(owner => owner.OwnerId.Equals(ownerId))
                .Include(ac => ac.Accounts)
                .FirstOrDefault();
        }

        public void UpdateOwner(Owner owner)
        {
            Update(owner);
        }
    }
}
