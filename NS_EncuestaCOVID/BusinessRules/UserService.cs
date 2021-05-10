using System.Configuration;
using static NS_EncuestaCOVID.Constants.Enums;
using System.DirectoryServices.AccountManagement;
using System;

namespace NS_EncuestaCOVID.BusinessRules
{
    public class UserService
    {
        private string _dominioPQP;
        private string _grupoDominioPermitido;
        private string _servidor;

        public UserService()
        {
            _dominioPQP = ConfigurationManager.AppSettings["dominioPQP"];
            _grupoDominioPermitido = ConfigurationManager.AppSettings["grupoDominioPermitido"];
            _servidor = ConfigurationManager.AppSettings["Servidor"];
        }

        public SignInStatus SingIn(string userName, string password, bool pIsPersitent)
        {
            try
            {
                bool _isValid = false;

                // conexion al dominio

                // OJO: Esta linea se deshabilita para instalar en PQP y se habilita para trabajo local por fuera del dominio PQP

                //PrincipalContext principalContext = new PrincipalContext(System.DirectoryServices.AccountManagement.ContextType.Domain,
                //    _servidor,
                //    _dominioPQP + userName,
                //    password);

                // Esta línea es la que funciona en PQP
                PrincipalContext principalContext = new PrincipalContext(System.DirectoryServices.AccountManagement.ContextType.Domain, _dominioPQP);

                UserPrincipal userDomain = UserPrincipal.FindByIdentity(principalContext, userName);

                // valido usuario y clave de dominio
                if (principalContext.ValidateCredentials(userName, password))
                {
                    _isValid = true;
                   //Se realiza if con debuf ya que no permite la consulta del grupo por maquina virtual. 
//#if !DEBUG
//                         // obtengo los grupos del usuario
//                    PrincipalSearchResult<Principal> groups = userDomain.GetAuthorizationGroups();

//                    // recorro los grupos
//                    foreach (Principal p in groups)
//                    {
//                        // me aseguro que sea un grupo
//                        if (p is GroupPrincipal)
//                        {
//                            // comparo el grupo del usuario con el permitido
//                            if (Convert.ToString(p) == _grupoDominioPermitido)
//                            {
//                                _isValid = true;
//                            }else{
                            
//                            _isValid = false;
//                            }
//                        }
//                    }
//#endif

                }
                if (!_isValid)
                {
                    return SignInStatus.Failure;
                }
                //_signInManager.SignIn(user, pIsPersitent, true);
                SignInStatus _signInStatus = SignInStatus.Success;
                // Se retorna el estado del login
                return _signInStatus;
            }
            catch(Exception e)
            {
                return SignInStatus.Failure;
            }
        }

    }
}