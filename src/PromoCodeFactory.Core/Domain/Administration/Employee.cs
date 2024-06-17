using System.Collections.Generic;

namespace PromoCodeFactory.Core.Domain.Administration
{
    public class Employee : BaseEntity, IMutableEntity<Employee>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string Email { get; set; }

        public List<Role> Roles { get; set; }

        public int AppliedPromocodesCount { get; set; }

        public void Update(Employee newData)
        {
            this.Email = newData.Email;
            this.FirstName = newData.FirstName;
            this.LastName = newData.LastName;
            this.AppliedPromocodesCount = newData.AppliedPromocodesCount;
            if (newData.Roles != null)
            {
                this.Roles.AddRange(newData.Roles);
            }
        }
    }
}