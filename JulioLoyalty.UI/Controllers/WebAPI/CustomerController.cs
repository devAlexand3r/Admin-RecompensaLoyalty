using JulioLoyalty.Business;
using JulioLoyalty.Business.Customer;
using JulioLoyalty.Business.Parameters;
using JulioLoyalty.Model;
using JulioLoyalty.UI.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JulioLoyalty.UI.Controllers.WebAPI
{
    [RoutePrefix("api/Customer")]
    public class CustomerController : ApiController
    {
        [AuditFilterApi, APIExceptionFilter]
        [HttpGet]
        [Route("CheckEmailTelCel")]
        public IHttpActionResult checkEmailTelCel(string email, string clave, string telcel, decimal participante_id)
        {
            string respuestaValidaCorreo = string.Empty;
            Business.Funciones.ValidaCorreo validaCorreo = new Business.Funciones.ValidaCorreo();
            Business.Funciones.validaTelCel validaTelCel = new Business.Funciones.validaTelCel();
            Entities.ValidaCorreo.csRespuestaValidacion _participante_Correo = new Entities.ValidaCorreo.csRespuestaValidacion();
            Entities.ValidaCorreo.csRespuestaValidacion _participante_TelCel = new Entities.ValidaCorreo.csRespuestaValidacion();
            Entities.ValidaCorreo.csRespuestaValidacion result = new Entities.ValidaCorreo.csRespuestaValidacion();
            if (email != null)
            { // 1
                if (email.ToLower().Contains("hotmail") || email.ToLower().Contains("gmail") || email.ToLower().Contains("outlook") || email.ToLower().Contains("yahoo"))
                { // 2
                    _participante_Correo = validaCorreo.BuscaParticipante_Email(email.Trim(), participante_id);
                    if (_participante_Correo.id == 0 && telcel.Trim().Length == 10) // 3 // No encontro un participante con mismo Correo, lo busca por Teléfono Celular
                        _participante_TelCel = validaTelCel.BuscaParticipante_TelCel(telcel.Trim(), participante_id);
                    if (!validaCorreo.ExisteEmailHistorico(email.Trim())) // No existe el correo en historico_emails
                    { // 3
                        Entities.ValidaCorreo.csValidaCorreo.ValidaCorreo csValida = new Entities.ValidaCorreo.csValidaCorreo.ValidaCorreo();
                        csValida = validaCorreo.Valida(email.Trim()); // Valida que sea correcto desde el Servicio
                        // Se puso eventual
                        //csValida.ErrorException = null;
                        //csValida.EmailVerification = new Entities.ValidaCorreo.csValidaCorreo.EmailVerification();
                        //csValida.EmailVerification.mailboxVerification = new Entities.ValidaCorreo.csValidaCorreo.MailboxVerification();
                        //csValida.EmailVerification.mailboxVerification.result = "ok";
                        // 
                        if (csValida.ErrorException == null)
                        { // 4 // No hubo error en el servicio
                            using (DbContextJulio db = new DbContextJulio())
                            { // using
                                var _participante_ = db.participante.Where(p => p.clave == clave).FirstOrDefault();
                                if (csValida.EmailVerification.mailboxVerification.result.ToLower() == "ok" || csValida.EmailVerification.mailboxVerification.result.ToLower() == "unverifiable")
                                { // 5
                                    // Inserta en la tabla historico_emails, el participante_id, correo y el usuario del alta
                                    // Se comento eventual
                                    validaCorreo.InsertaHistoricoEmail(_participante_ == null ? 0 : _participante_.id, email.Trim(), Guid.Parse(User.Identity.GetUserId()), "OK");
                                    //
                                    result.message = "Correo valido";
                                } // 5
                                else
                                { // 5
                                    // Inserta en la tabla historico_emails, el participante_id, correo y el usuario del alta
                                    validaCorreo.InsertaHistoricoEmail(_participante_ == null ? 0 : _participante_.id, email.Trim(), Guid.Parse(User.Identity.GetUserId()), "NO");
                                    result.message = "Correo invalido";
                                } // 5
                                respuestaValidaCorreo = csValida.EmailVerification.mailboxVerification.result + " | " + csValida.EmailVerification.mailboxVerification.reason;
                                // usuario validado por la dirección de correo                                
                                result.result = csValida.EmailVerification.mailboxVerification.result.ToLower();
                                if (result.result == "unverifiable" || result.result == "ok") // 5
                                { // 5 Es correcto
                                    result.result = "ok";
                                    result.status = 0;
                                }
                                else // 5 Es Incorrecto
                                {
                                    result.result = "El correo electrónico no es válido";
                                    result.status = 1;
                                }
                                result.reason = csValida.EmailVerification.mailboxVerification.reason;
                            } // using
                        } // 4
                        else
                        { // 4
                            result.status = 1; // ErrorCorreo
                            result.result = "El correo electrónico no es válido";
                            result.errorException = csValida.ErrorException;
                            result.message = "errorCorreo";
                            result.errorException = respuestaValidaCorreo;
                        } // 4
                    } // 3
                    else
                    { // 3 // Existe el Correo en historico_emails verifica su status
                      // Es necesario saber su status
                        string status = validaCorreo.ObtieneStatusEmail(email.Trim());
                        if (status.ToUpper() != "OK")
                        { // 4
                            result.status = 1; // ErrorCorreo
                            result.result = "El correo electrónico no es válido";
                            result.errorException = "errorCorreo";
                            result.message = "errorCorreo";
                        } // 4
                        else
                        { // 4
                            result.status = 0;
                            result.result = "ok";
                            result.message = "El correo ya existe en historico de emails, no es necesario validarlo";
                        } // 4
                    } // 3
                    // Si el correo es correcto hay que validar el Teléfono Celular
                    if (result.status == 0)
                    { // 3
                        if (telcel.Trim().Length == 10) // Solo va validar números de 10 digitos
                        { // 4
                            if (validaTelCel.ExisteTelCelNoPermitido(telcel.Trim()))
                            { // 5
                                result.status = 1; // ErrorTelCel
                                result.result = "El Teléfono Celular No Está Permitido";
                                result.errorException = "errorTelCel";
                                result.message = "errorTelCel";
                            } // 5
                        } // 4
                    } // 3
                } // 2
                else if (email.ToLower().Contains("@grupojulio") || email.ToLower().Contains("@proymoda")) // No permite agregar correos con estos dominios
                { // 2
                    result.status = 1; // Error
                    result.result = "El correo electrónico no es válido";
                    result.message = "Correo invalido tiene dominios de @grupojulio y @proymoda";
                } // 2
                else if (email.ToLower().Contains("@gamil") || email.ToLower().Contains("@gmil") || email.ToLower().Contains("@hormail")
                      || email.ToLower().Contains("@hotmil") || email.ToLower().Contains("@hotmal") || email.ToLower().Contains("@otlok")
                      || email.ToLower().Contains("@hatmal") || email.ToLower().Contains("@ootlok") || email.ToLower().Contains("@hmail")
                      || email.ToLower().Contains("@gimail") || email.ToLower().Contains("@gmai."))
                // No permite agregar correos por error de dedo
                { // 2
                    result.status = 1; // Error
                    result.result = "El correo electrónico no es válido";
                    result.message = "Correo invalido tiene dominios similares a los de @gmail, @outlook y @hotmail";
                } // 2
                else if (email.ToLower().Contains("@correo.com") || email.ToLower().Contains("@no correo.com") || email.ToLower().Contains("@nocorreo.com") || email.ToLower().Contains("@mil.com"))
                { // 2
                    result.status = 1; // Error
                    result.result = "El correo electrónico no es válido";
                    result.message = "Correo invalido tiene dominios incorrectos";
                } // 2
                else
                { // 2
                    result.status = 0;
                    result.result = "ok";
                    result.message = "El correo tiene dominios diferentes a hotmail, gmail, outlook y yahoo, no es necesario validarlo";
                    // Si el correo es correcto hay que validar el Teléfono Celular
                    if (telcel.Trim().Length == 10) // Solo va validar números de 10 digitos
                    { // 3
                        if (validaTelCel.ExisteTelCelNoPermitido(telcel.Trim()))
                        { // 4
                            result.status = 1; // ErrorTelCel
                            result.result = "El Teléfono Celular No Está Permitido";
                            result.errorException = "errorTelCel";
                            result.message = "errorTelCel";
                        } // 4
                    } // 3
                    if (result.status == 0)
                    { // 3
                        _participante_Correo = validaCorreo.BuscaParticipante_Email(email.Trim(), participante_id);
                        if (telcel.Trim().Length == 10) // 4 // Solo va validar números de 10 digitos
                        { // No encontro un participante con mismo Correo, lo busca por Teléfono Celular
                            _participante_TelCel = validaTelCel.BuscaParticipante_TelCel(telcel.Trim(), participante_id);
                            if (_participante_TelCel.id == 0) // 5
                            {
                                result.result = "ok";
                                result.message = "El correo tiene dominios diferentes a hotmail, gmail, outlook y yahoo, no es necesario validarlo";
                            }
                        } // 4
                    } // 3
                } // 2
            } // 1
            else // Valida Teléfono Celular
            { // 1
                if (telcel.Trim().Length == 10)
                { // 2
                    _participante_TelCel = validaTelCel.BuscaParticipante_TelCel(telcel.Trim(), participante_id);
                    if (validaTelCel.ExisteTelCelNoPermitido(telcel.Trim()))
                    { // 3
                        result.status = 1; // ErrorTelCel
                        result.result = "El Teléfono Celular No Está Permitido";
                        result.errorException = "errorTelCel";
                        result.message = "errorTelCel";
                    } // 3
                    else
                    { // 3
                        result.status = 0;
                        result.result = "ok";
                    } // 3
                } // 2
            } // 1
            if (_participante_Correo.id != 0) // 1
            {
                result.id = _participante_Correo.id;
                result.clave = _participante_Correo.clave;
                result.nombre = _participante_Correo.nombre;
                result.apellido_paterno = _participante_Correo.apellido_paterno;
                result.apellido_materno = _participante_Correo.apellido_materno;
                result.correo_electronico = _participante_Correo.correo_electronico;
                result.fecha_nacimiento = _participante_Correo.fecha_nacimiento;
                result.telefono_celular = null;
            }
            else if (_participante_TelCel.id != 0) // 1
            {
                result.id = _participante_TelCel.id;
                result.clave = _participante_TelCel.clave;
                result.nombre = _participante_TelCel.nombre;
                result.apellido_paterno = _participante_TelCel.apellido_paterno;
                result.apellido_materno = _participante_TelCel.apellido_materno;
                result.correo_electronico = null;
                result.fecha_nacimiento = _participante_TelCel.fecha_nacimiento;
                result.telefono_celular = _participante_TelCel.telefono_celular;
            }
            return Json(result);
        }

        [AuditFilterApi]
        [HttpPost]
        [Route("AddCustomer")]
        public IHttpActionResult addCustomer(RequestCustomer request)
        {
            request.UserName = User.Identity.Name;
            request.URL = this.Url.Link("Default", new { Controller = "Account", Action = "Login" });
            IRepositoryCustomer customer = new Customer();
            return Json(customer.AddCustomer(request));
        }

        [AuditFilterApi, APIExceptionFilter]
        [HttpPost]
        [Route("UpdateCustomer")]
        public IHttpActionResult updateCustomer(RequestCustomer request)
        {
            request.UserName = User.Identity.Name;
            IRepositoryCustomer customer = new Customer();
            return Json(customer.UpdateCustomer(request));
        }

        [AuditFilterApi, APIExceptionFilter]
        [HttpPost]
        [Route("UnifyCustomer")]
        public string unifyCustomer(RequestCustomer request)
        {
            request.UserName = User.Identity.Name;
            request.URL = this.Url.Link("Default", new { Controller = "Account", Action = "Login" });
            IRepositoryCustomer customer = new Customer();
            ResultJson jsonResult = new ResultJson();
            // Primero Hace la Unificación y si es Correcto hace el Registro de Socia
            using (DbContextJulio db = new DbContextJulio())
            {
                var aspnetusers_id = db.AspNetUsers.Where(s => s.UserName == User.Identity.Name).FirstOrDefault().Id;
                dynamic _participante = null;
                if (!string.IsNullOrEmpty(request.Correo))
                {
                    _participante = (from p in db.participante
                                     join t in db.transaccion on p.id equals t.participante_id
                                     where p.correo_electronico == request.Correo.Trim() && p.status_participante_id != 13 && p.status_participante_id != 14 && t.tipo_transaccion_id == 1 && p.id == request.Id
                                     select new
                                     {
                                         id = p.id,
                                         clave = p.clave,
                                         fecha = t.fecha
                                     }
                                     ).OrderByDescending(t => t.fecha).FirstOrDefault();
                }
                if (_participante == null)
                {
                    if (!string.IsNullOrEmpty(request.Tel_celular) && request.Tel_celular.Trim() != "0")
                    {
                        _participante = (from p in db.participante
                                         join pt in db.participante_telefono on p.id equals pt.participante_id
                                         join t in db.transaccion on p.id equals t.participante_id
                                         where pt.telefono == request.Tel_celular.Trim() && pt.tipo_telefono_id == 3 && p.status_participante_id != 13 && p.status_participante_id != 14 && t.tipo_transaccion_id == 1 && p.id == request.Id
                                         select new
                                         {
                                             id = p.id,
                                             clave = p.clave,
                                             fecha = t.fecha
                                         }).OrderByDescending(t => t.fecha).FirstOrDefault();
                    }
                }
                // Conserva el status del participante actual, en caso de haber un error
                decimal status_participante_id = customer.ObtieneStatusPartipanteId(request);
                // Actualiza status participante como activo
                customer.ActualizaStatusParticipante(request, 2);
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@transferencia", "UNIFICACION");
                parameters.Add("@NoTarjeta", _participante.clave);
                parameters.Add("@NuevaTarjeta", request.Num_tarjeta);
                parameters.Add("@participante_id", _participante.id);
                parameters.Add("@aspnetusers_id", aspnetusers_id);
                DataSet setTables = db.GetDataSet("[dbo].[usp_transfiere_tarjeta]", CommandType.StoredProcedure, parameters);
                if (setTables.Tables[0].Rows[0]["errorId"].ToString() == "0")
                {
                    // Actualiza status participante como estaba. Esto para que permita pasar el proceso de actualizar socia
                    customer.ActualizaStatusParticipante(request, status_participante_id);
                    jsonResult = customer.UnifyCustomer(request);
                    jsonResult.jsonObject = JsonConvert.SerializeObject(setTables.Tables[0]);
                }
                else // Ocurrio un error se regresa como estaba el status del participante
                {
                    // Actualiza status participante como estaba
                    customer.ActualizaStatusParticipante(request, status_participante_id);
                    jsonResult.jsonObject = JsonConvert.SerializeObject(setTables.Tables[0]);
                }
            }
            return JsonConvert.SerializeObject(jsonResult);
        }

        [AuditFilterApi, APIExceptionFilter]
        [HttpPost]
        [Route("Update/UnifyCustomer")]
        public string updateUnifyCustomer(RequestCustomer request)
        {
            request.UserName = User.Identity.Name;
            request.URL = this.Url.Link("Default", new { Controller = "Account", Action = "Login" });
            IRepositoryCustomer customer = new Customer();
            ResultJson jsonResult = new ResultJson();
            // Primero Hace la Unificación y si es Correcto hace la Actualización de Socia
            using (DbContextJulio db = new DbContextJulio())
            {
                var aspnetusers_id = db.AspNetUsers.Where(s => s.UserName == User.Identity.Name).FirstOrDefault().Id;
                dynamic _participante = null;
                if (!string.IsNullOrEmpty(request.Correo))
                {
                    _participante = (from p in db.participante
                                     join t in db.transaccion on p.id equals t.participante_id
                                     where p.correo_electronico == request.Correo.Trim() && p.status_participante_id != 13 && p.status_participante_id != 14 && t.tipo_transaccion_id == 1 && p.id != request.Id
                                     select new
                                     {
                                         id = p.id,
                                         clave = p.clave,
                                         fecha = t.fecha
                                     }
                                     ).OrderByDescending(t => t.fecha).FirstOrDefault();
                }
                if (_participante == null)
                {
                    if (!string.IsNullOrEmpty(request.Tel_celular) && request.Tel_celular.Trim() != "0")
                    {
                        _participante = (from p in db.participante
                                         join pt in db.participante_telefono on p.id equals pt.participante_id
                                         join t in db.transaccion on p.id equals t.participante_id
                                         where pt.telefono == request.Tel_celular.Trim() && pt.tipo_telefono_id == 3 && p.status_participante_id != 13 && p.status_participante_id != 14 && t.tipo_transaccion_id == 1 && p.id != request.Id
                                         select new
                                         {
                                             id = p.id,
                                             clave = p.clave,
                                             fecha = t.fecha
                                         }).OrderByDescending(t => t.fecha).FirstOrDefault();
                    }
                }
                // Conserva el status del participante actual, en caso de haber un error
                decimal status_participante_id = customer.ObtieneStatusPartipanteId(request);
                // Actualiza status participante como activo
                customer.ActualizaStatusParticipante(request, 2);
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@transferencia", "UNIFICACION");
                parameters.Add("@NoTarjeta", _participante.clave);
                parameters.Add("@NuevaTarjeta", request.Num_tarjeta);
                parameters.Add("@participante_id", _participante.id);
                parameters.Add("@aspnetusers_id", aspnetusers_id);
                DataSet setTables = db.GetDataSet("[dbo].[usp_transfiere_tarjeta]", CommandType.StoredProcedure, parameters);
                if (setTables.Tables[0].Rows[0]["errorId"].ToString() == "0")
                {
                    // Actualiza status participante como estaba. Esto para que permita pasar el proceso de actualizar socia
                    customer.ActualizaStatusParticipante(request, status_participante_id);
                    jsonResult = customer.UpdateCustomer(request);
                    jsonResult.jsonObject = JsonConvert.SerializeObject(setTables.Tables[0]);
                }
                else // Ocurrio un error se regresa como estaba el status del participante
                {
                    // Actualiza status participante como estaba
                    customer.ActualizaStatusParticipante(request, status_participante_id);
                    jsonResult.jsonObject = JsonConvert.SerializeObject(setTables.Tables[0]);
                }
            }
            return JsonConvert.SerializeObject(jsonResult);
        }

        [AuditFilterApi, APIExceptionFilter]
        [HttpPost]
        [Route("AddUser")]
        public IHttpActionResult addUser(RequesUser request)
        {
            IRepositoryCustomer customer = new Customer();
            return Json(customer.AddUser(request));
        }

        [AuditFilterApi, APIExceptionFilter]
        [HttpPost]
        [Route("UpdateUser")]
        public IHttpActionResult updateUser(RequesUser request)
        {
            IRepositoryCustomer customer = new Customer();
            return Json(customer.UpdateUser(request));
        }

        [AuditFilterApi, APIExceptionFilter]
        [HttpGet]
        [Route("Find")] // Busqueda de socias
        public string customerFind(string parameter)
        {
            DataTable table = new DataTable();
            using (DbContextJulio db = new DbContextJulio())
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@Parametro", parameter);
                DataSet setTables = db.GetDataSet("[dbo].[usp_buscar_clientes]", CommandType.StoredProcedure, parameters);
                table = setTables.Tables[0];
                return JsonConvert.SerializeObject(table);
            }
        }

        [AuditFilterApi, APIExceptionFilter]
        [HttpGet]
        [Route("FindByUser")] // consultar datos del participante
        public IHttpActionResult customerFindById(int id)
        {
            using (DbContextJulio db = new DbContextJulio())
            {
                // Consultar saldo de la socia
                var _saldo = db.Database.SqlQuery<int>("EXEC sp_obtiene_saldo_participante @participante_id", new SqlParameter { ParameterName = "participante_id", Value = id }).ToList()[0];

                // Consultar socia por medio de su identificador
                var _socia = db.participante.Where(s => s.id == id).FirstOrDefault();
                // Convertir la primera letra en mayúsculas de cada palabra
                string nombre = string.IsNullOrEmpty(_socia.nombre) == true ? "" : CultureInfo.InvariantCulture.TextInfo.ToTitleCase(_socia.nombre.ToLower());
                string apellido_paterno = string.IsNullOrEmpty(_socia.apellido_paterno) == true ? "" : CultureInfo.InstalledUICulture.TextInfo.ToTitleCase(_socia.apellido_paterno.ToLower());
                string apellido_materno = string.IsNullOrEmpty(_socia.apellido_materno) == true ? "" : CultureInfo.InstalledUICulture.TextInfo.ToTitleCase(_socia.apellido_materno.ToLower());

                // Consultar la infomación restante de la socia
                var result = db.participante.Where(s => s.id == id).Select(s => new
                {
                    s.id,
                    s.clave,
                    nombre,
                    apellido_paterno,
                    apellido_materno,
                    s.correo_electronico,
                    s.distribuidor_id,
                    s.sexo_id,
                    s.fecha_nacimiento,
                    s.momento_favorito,
                    s.frecuencia_compra,
                    s.participante_direccion.FirstOrDefault().estado,
                    s.participante_direccion.FirstOrDefault().municipio,
                    s.participante_direccion.FirstOrDefault().colonia,
                    s.participante_direccion.FirstOrDefault().codigo_postal,
                    telefonos = s.participante_telefono.Where(t => t.participante_id == s.id).Select(t => new
                    {
                        t.id,
                        t.tipo_telefono_id,
                        t.lada,
                        t.telefono,
                        t.extension
                    }).ToList(),
                    s.ocupacion_id,
                    nivel = db.participante_nivel.Where(x => x.participante_id == s.id).FirstOrDefault().nivel_id,
                    status = s.status_participante.descripcion,
                    saldo = _saldo,
                    participante_nivel = db.participante_nivel.Where(x => x.participante_id == s.id).Select(x => new
                    {
                        x.id,
                        x.participante_id,
                        x.nivel_id,
                        x.fecha_final,
                        x.fecha_inicial,
                        x.tickets,
                        x.importe
                    }).ToList(),

                    s.status_participante_id
                }).ToList();
                return Json(result);
            }
        }

        [AuditFilterApi, APIExceptionFilter]
        [HttpGet]
        [Route("ConsultaTransaccion")]
        public string consultarTransaccion(int id)
        {
            using (DbContextJulio db = new DbContextJulio())
            {
                try
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("@participante_id", id);
                    parameters.Add("@usuario_id", User.Identity.GetUserId());
                    DataSet setTables = db.GetDataSet("[dbo].[usp_consultar_transaccion]", CommandType.StoredProcedure, parameters);
                    return JsonConvert.SerializeObject(setTables.Tables[0]);
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }

        [AuditFilterApi, APIExceptionFilter]
        [HttpGet]
        [Route("CTransaccionDetalle")]
        public string cTransaccionDetalle(int id)
        {
            using (DbContextJulio db = new DbContextJulio())
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@transaccion_id", id);
                DataSet setTables = db.GetDataSet("[dbo].[usp_consultar_transacciondetalle]", CommandType.StoredProcedure, parameters);
                return JsonConvert.SerializeObject(setTables.Tables[0]);
            }
        }

        [AuditFilterApi, APIExceptionFilter]
        [HttpGet]
        [Route("CTransaccionComentarios")]
        public string cTransaccionComentarios(decimal transaccion_id)
        {
            using (DbContextJulio db = new DbContextJulio())
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@transaccion_id", transaccion_id);
                DataSet setTables = db.GetDataSet("[dbo].[usp_consultar_transaccion_comentarios]", CommandType.StoredProcedure, parameters);
                return JsonConvert.SerializeObject(setTables.Tables[0]);
            }
        }

        [AuditFilterApi, APIExceptionFilter]
        [HttpGet]
        [Route("Participante/Direccion")]
        public string participanteDireccion(int id)
        {
            using (DbContextJulio db = new DbContextJulio())
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@participante_id", id);
                DataSet setTables = db.GetDataSet("[dbo].[usp_buscar_participantedireccion]", CommandType.StoredProcedure, parameters);
                return JsonConvert.SerializeObject(setTables.Tables[0]);
            }
        }

        [AuditFilterApi, APIExceptionFilter]
        [HttpPost]
        [Route("Agregar/Transaccion")]
        public IHttpActionResult agregarTransaccion(RequestTransaction request)
        {
            request.userName = User.Identity.Name;
            Business.Transacciones.IRepositorioTransaccion repos = new Business.Transacciones.Transaccion();
            return Json(repos.AgregarTransaccion(request));
        }

        [AuditFilterApi, APIExceptionFilter]
        [HttpGet]
        [Route("Consultar/Participantes")]
        public string consultarParticipantes()
        {
            using (DbContextJulio db = new DbContextJulio())
            {
                DataSet setTables = db.GetDataSet("[dbo].[usp_consultar_participantes]", CommandType.StoredProcedure);
                return JsonConvert.SerializeObject(setTables.Tables[0]);
            }
        }

        [AuditFilterApi, APIExceptionFilter]
        [HttpPost]
        [Route("Actualizar/Status")]
        public IHttpActionResult actualizarStatus(RequestComments request)
        {
            request.userName = User.Identity.Name;
            IRepositoryCustomer customer = new Customer();
            return Json(customer.UpdateStutus(request));
        }

        [AuditFilterApi, APIExceptionFilter]
        [HttpPost]
        [Route("Remplazar/Unificar/Menbresia")]
        public string remUniMenbresia(RequestRepOtUnif request)
        {
            using (DbContextJulio db = new DbContextJulio())
            {
                var aspnetusers_id = db.AspNetUsers.Where(s => s.UserName == User.Identity.Name).FirstOrDefault().Id;
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@transferencia", request.tranferencia);
                parameters.Add("@NoTarjeta", request.NoTarjeta);
                parameters.Add("@NuevaTarjeta", request.NuevaTarjeta);
                parameters.Add("@participante_id", request.participante_id);
                parameters.Add("@aspnetusers_id", aspnetusers_id);
                DataSet setTables = db.GetDataSet("[dbo].[usp_transfiere_tarjeta]", CommandType.StoredProcedure, parameters);
                return JsonConvert.SerializeObject(setTables.Tables[0]);
            }
        }

        [AuditFilterApi, APIExceptionFilter]
        [HttpPost]
        [Route("Agregar/Acumulacion")]
        public string agregarAcumulacion(RequestActiva request)
        {
            using (DbContextJulio db = new DbContextJulio())
            {
                var UserId = db.AspNetUsers.Where(s => s.UserName == User.Identity.Name).FirstOrDefault().Id;
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@NoTarjeta", request.NoTarjeta);
                parameters.Add("@NoTienda", request.NoTienda);
                parameters.Add("@NoTicket", request.NoTicket);
                parameters.Add("@UserId", UserId);
                DataSet setTables = db.GetDataSet("[dbo].[usp_aplica_acumulacion]", CommandType.StoredProcedure, parameters);
                return JsonConvert.SerializeObject(setTables.Tables[0]);
            }
        }

        [AuditFilterApi, APIExceptionFilter]
        [HttpPost]
        [Route("Activar/Tarjeta")]
        public string activarTarjeta(RequestActiva request)
        {
            using (DbContextJulio db = new DbContextJulio())
            {
                var UserId = db.AspNetUsers.Where(s => s.UserName == User.Identity.Name).FirstOrDefault().Id;
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@NoTarjeta", request.NoTarjeta);
                parameters.Add("@NoTienda", request.NoTienda);
                parameters.Add("@NoTicket", request.NoTicket);
                parameters.Add("@UserId", UserId);
                DataSet setTables = db.GetDataSet("[dbo].[usp_activar_tarjeta]", CommandType.StoredProcedure, parameters);
                return JsonConvert.SerializeObject(setTables.Tables[0]);
            }
        }

        [AuditFilterApi, APIExceptionFilter]
        [HttpGet]
        [Route("Usuario/Distribuidor")]
        public string usuarioDistribuidor()
        {
            using (DbContextJulio db = new DbContextJulio())
            {
                var UserId = db.AspNetUsers.Where(s => s.UserName == User.Identity.Name).FirstOrDefault().Id;
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@UserId", UserId);
                DataSet setTables = db.GetDataSet("[dbo].[usp_consultar_usuario_distribuidor]", CommandType.StoredProcedure, parameters);
                return JsonConvert.SerializeObject(setTables.Tables[0]);
            }
        }
    }
}