using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyBuddy
{
    class User
    {
        private String username;
        private String password;
        private String email;
        private String firstName;
        private String lastName;
        private String timestamp;
        private double latitude;
        private double longitude;

        public void setUsername(String username)
        {
            this.username = username;
        }

        public String getUsername()
        {
            return username;
        }

        public void setPassword(String password)
        {
            this.password = password;
        }

        public String getPassword()
        {
            return password;
        }
        public void setEmail(String email)
        {
            this.email = email;
        }

        public String getEmail()
        {
            return email;
        }

        public void setFirstName(String firstName)
        {
            this.firstName = firstName;
        }

        public String getFirstName()
        {
            return firstName;
        }

        public void setLastName(String lastName)
        {
            this.lastName = lastName;
        }

        public String getLastName()
        {
            return lastName;
        }

        public void setTimestamp(String timestamp)
        {
            this.timestamp = timestamp;
        }

        public String getTimestamp()
        {
            return timestamp;
        }

        public void setLatitude(double latitude)
        {
            this.latitude = latitude;
        }

        public double getLatitude()
        {
            return latitude;
        }

        public void setLongitude(double longitude)
        {
            this.longitude = longitude;
        }

        public double getLongitude()
        {
            return longitude;
        }
    }
}
