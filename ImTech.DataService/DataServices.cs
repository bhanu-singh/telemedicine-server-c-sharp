using ImTech.DataBase;
using ImTech.DataServices.File;
using ImTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ImTech.DataServices
{
    public class DataServices
    {
        public DataServices(IDataBaseService dataBaseService)
        {
            LoginService = new LoginService(dataBaseService);
            UserService = new UserService(dataBaseService);
            DoctorService = new DoctorService(dataBaseService);
            ConsultationService = new ConsultationService(dataBaseService);
            SecureFileService =  new SecureFileService(dataBaseService, FileServiceFactory.GetFileSystem());
            NonSecureFileService = new NonSecureFileService(dataBaseService, FileServiceFactory.GetFileSystem());
        }

        public UserService UserService
        {
            get;
            private set;
        }

        public DoctorService DoctorService
        {
            get;
            private set;
        }

        public SecureFileService SecureFileService
        {
            get;
            private set;
        }

        public NonSecureFileService NonSecureFileService
        {
            get;
            private set;
        }

        public ConsultationService ConsultationService
        {
            get;
            private set;
        }

        public LoginService LoginService
        {
            get;
            private set;
        }
    }
}
