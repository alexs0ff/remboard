using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Romontinka.Server.Protocol.AuthMessages;
using Romontinka.Server.Protocol.SynchronizationMessages;

namespace Romontinka.Server.Protocol
{
    /// <summary>
    /// Сериализатор и десериализатор протокола 
    /// </summary>
    public sealed class ProtocolSerializer
    {
        private readonly string _version;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Romontinka.Server.Protocol.ProtocolSerializer"/> class.
        /// </summary>
        /// <param name="version">Номер версии.</param>
        public ProtocolSerializer(string version)
        {
            _version = version;
        }

        #region Common

        /// <summary>
        /// Парсит сообщение для определения информации по нему.
        /// </summary>
        /// <param name="xml">Данные.</param>
        /// <returns>Информация.</returns>
        public MessageInfo GetMessageInfo(string xml)
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(xml);
                return GetInfo(doc);
            }
            catch (XmlException)
            {
                throw new MessageParseException("Сообщение не соответсвует формату xml","xml");
                
            } //try
        }

        /// <summary>
        /// Возвращает разобранную информацию с сообщения протокола или null, если что-то не верно.
        /// </summary>
        /// <param name="xml">Данные.</param>
        /// <returns>Информация или null</returns>
        public MessageInfo GetMessageInfoOrNull(string xml)
        {
            try
            {
                return GetMessageInfo(xml);
            }
            catch (Exception)
            {

                return null;
            } //try
        }

        /// <summary>
        /// Содержит константы для Xml.
        /// </summary>
        private static class XmlConstants
        {
            public const string MessageRoot = "RemontinkaMessage";

            public const string SignData = "SignData";

            public const string UserID = "UserID";

            public const string RequestInfo = "RequestInfo";
            
            public const string Version = "Version";

            public const string RequestKind = "Kind";

            public const string VersionXPath = MessageRoot + "/" + RequestInfo + "/" + Version;

            public const string RequestKindXPath = MessageRoot + "/" + RequestInfo + "/" + RequestKind;
        }

        /// <summary>
        /// Создает сообщение по параметрам.
        /// </summary>
        /// <param name="request">Текущий запрос..</param>
        /// <param name="createBodyMethod">Метод который будет вызваон для создания дополнительных параметров запроса.</param>
        /// <returns>Сформированный xml запрос.</returns>
        private string CreateMessage<T>( T request, Action<XmlWriter, T> createBodyMethod)
            where T : MessageBase
        {
            if (string.IsNullOrWhiteSpace(request.Version))
            {
                request.Version = _version;
            } //if
            var result = new StringBuilder();

            var settings = new XmlWriterSettings { Indent = true };
            using (var xmlWriter = XmlWriter.Create(result, settings))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement(XmlConstants.MessageRoot);
                xmlWriter.WriteStartElement(XmlConstants.RequestInfo);
                xmlWriter.WriteElementString(XmlConstants.Version, request.Version);
                xmlWriter.WriteElementString(XmlConstants.RequestKind, request.Kind.ToString());
                xmlWriter.WriteEndElement();

                if (createBodyMethod != null)
                {
                    createBodyMethod(xmlWriter, request);
                } //if

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
            }

            return result.ToString();
        }

        /// <summary>
        /// Производит разбор сообщения.
        /// </summary>
        /// <typeparam name="T">Тип сообщения.</typeparam>
        /// <param name="xml">Xml для разбора. </param>
        /// <param name="parseBodyMethod">Метод парсинга тела сообщения.</param>
        /// <returns>Сообщение.</returns>
        private T ParseMessage<T>(string xml,Action<XmlDocument, T> parseBodyMethod)
            where T : MessageBase,new()
        {
            var result = new T();

            var doc = new XmlDocument();
            doc.LoadXml(xml);
            var info = GetInfo(doc);

            if (info.Kind!=result.Kind)
            {
                throw new MessageParseException(
                    string.Format("Не верный тип для разбора должен быть {0}, а был {1}", result.Kind, info.Kind),
                    XmlConstants.RequestKind);
            } //if

            result.Version = info.Version;

            if (parseBodyMethod!=null)
            {
                parseBodyMethod(doc, result);
            } //if

            return result;
        }

        /// <summary>
        /// Возвращает информацию по сообщениям.
        /// </summary>
        /// <param name="document">Документ.</param>
        /// <returns>Информация.</returns>
        private MessageInfo GetInfo(XmlDocument document)
        {
            var result = new MessageInfo();

            result.Version = ReadRequiredElement(document, XmlConstants.VersionXPath, "Нет номера версии");
            var kindResult = ReadRequiredElement(document, XmlConstants.RequestKindXPath, "Нет типа сообщения");

            MessageKind messageKind;

            if (!Enum.TryParse(kindResult, true, out messageKind))
            {
                throw new MessageParseException("Нет такого типа сообщения " + kindResult, XmlConstants.RequestKind);
            }

            result.Kind = messageKind;
            return result;
        }

        /// <summary>
        /// Считывает значение обязательного элемента.
        /// </summary>
        /// <param name="document">Xml документ.</param>
        /// <param name="xPath">Путь к нему.</param>
        /// <param name="message">Сообщение.</param>
        /// <returns>Прочитанное значение.</returns>
        private string ReadRequiredElement(XmlNode document, string xPath, string message)
        {
            var node = document.SelectSingleNode(xPath);
            if (node==null)
            {
                throw new MessageParseException(message,xPath);
            } //if

            return node.InnerText;
        }

        /// <summary>
        /// Считывает значение обязательного элемента.
        /// </summary>
        /// <param name="document">Xml документ.</param>
        /// <param name="xPath">Путь к нему.</param>
        /// <returns>Прочитанное значение.</returns>
        private string ReadRequiredElement(XmlNode document, string xPath)
        {
            return ReadRequiredElement(document, xPath, "Нет элемента по пути " + xPath);
        }

        /// <summary>
        /// Производит чтение атрибутов.
        /// </summary>
        /// <param name="node">Нод содержащий атрибуты.</param>
        /// <param name="name">Имя атрибута.</param>
        /// <returns>Значение или null, если нет атрибута.</returns>
        private string ReadAttribute(XmlNode node, string name)
        {
            string value = null;

            if (node != null && node.Attributes != null && node.Attributes[name] != null)
            {
                var attr = node.Attributes[name];
                value = attr.Value;
            } //if

            return value;
        }

        /// <summary>
        /// Записывает значение атрибута.
        /// </summary>
        /// <param name="writer">Xml писатель.</param>
        /// <param name="name">Имя атрибута.</param>
        /// <param name="value">Значение атрибута.</param>
        private void WriteAttribute(XmlWriter writer, string name, string value)
        {
            if (value!=null)
            {
                writer.WriteAttributeString(name,value);
            } //if
        }

        #endregion Common

        #region RegisterPublicKey

        /// <summary>
        /// Производит сериализацию запроса на регистрацию публичного ключа.
        /// </summary>
        /// <param name="request">Запрос регистрации публичного ключа.</param>
        /// <returns>Сериализованные данные.</returns>
        public string Serialize(RegisterPublicKeyRequest request)
        {
            return CreateMessage(request, (writer, keyRequest) =>
                                          {
                                              writer.WriteElementString("UserLogin", request.UserLogin);
                                              writer.WriteElementString("EventDate", Utils.DateTimeToString(request.EventDate));
                                              writer.WriteStartElement("PublicKey");
                                              writer.WriteCData(request.PublicKeyData);
                                              writer.WriteEndElement();
                                              writer.WriteElementString("KeyNotes", request.KeyNotes);
                                              writer.WriteElementString("ClientUserDomainID", Utils.GuidToString(request.ClientUserDomainID));
                                          });
        }

        /// <summary>
        /// Производит десериализацию запроса регистрации публичного ключа.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Запрос.</returns>
        public RegisterPublicKeyRequest DeserializeRegisterPublicKeyRequest(string data)
        {
            return ParseMessage<RegisterPublicKeyRequest>(data, (document, request) =>
                                                                {
                                                                    request.KeyNotes = ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + "KeyNotes");
                                                                    request.UserLogin = ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + "UserLogin");
                                                                    request.PublicKeyData = ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + "PublicKey");
                                                                    request.EventDate =
                                                                        Utils.ParseDateTime(ReadRequiredElement(
                                                                            document,
                                                                            XmlConstants.MessageRoot + "/" + "EventDate")) ??
                                                                        DateTime.Today;
                                                                    request.ClientUserDomainID = Utils.ParseGuid(ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + "ClientUserDomainID"));
                                                                });
        }

        #endregion RegisterPublicKey

        #region ErrorResponse

        /// <summary>
        /// Производит сериализацию ошибочного ответа.
        /// </summary>
        /// <param name="message">Ответ с ошибкой.</param>
        /// <returns>Сериализованные данные.</returns>
        public string Serialize(ErrorResponse message)
        {
            return CreateMessage(message, (writer, keyRequest) => writer.WriteElementString("Description", message.Description));
        }

        /// <summary>
        /// Производит десериализацию ответа с ошибкой.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Ответ с ошибкой.</returns>
        public ErrorResponse DeserializeErrorResponse(string data)
        {
            return ParseMessage<ErrorResponse>(data, (document, request) =>
            {
                request.Description = ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + "Description");
                
            });
        }

        #endregion ErrorResponse

        #region AuthErrorResponse

        /// <summary>
        /// Производит сериализацию ошибочного ответа авторизации.
        /// </summary>
        /// <param name="message">Ответ с ошибкой авторизации.</param>
        /// <returns>Сериализованные данные.</returns>
        public string Serialize(AuthErrorResponse message)
        {
            return CreateMessage(message, (writer, keyRequest) => writer.WriteElementString("Message", message.Message));
        }

        /// <summary>
        /// Производит десериализацию ответа с ошибкой автороизации.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Ответ с ошибкой авторизации.</returns>
        public AuthErrorResponse DeserializeAuthErrorResponse(string data)
        {
            return ParseMessage<AuthErrorResponse>(data, (document, request) =>
            {
                request.Message = ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + "Message");


            });
        }

        #endregion AuthErrorResponse

        #region RegisterPublicKeyResponse

        /// <summary>
        /// Производит сериализацию ответа при регистрации публичного ключа.
        /// </summary>
        /// <param name="message">Ответ с результатов регистрации публичного ключа.</param>
        /// <returns>Сериализованные данные.</returns>
        public string Serialize(RegisterPublicKeyResponse message)
        {
            return CreateMessage(message, (writer, keyRequest) =>
                                          {
                                              writer.WriteElementString("Number", message.Number);
                                              writer.WriteElementString("UserDomainID", Utils.GuidToString(message.UserDomainID));
                                          });
        }

        /// <summary>
        /// Производит десериализацию ответа с результатом регистрации публичного ключа.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Ответ результатом публичного ключа.</returns>
        public RegisterPublicKeyResponse DeserializeRegisterPublicKeyResponse(string data)
        {
            return ParseMessage<RegisterPublicKeyResponse>(data, (document, request) =>
            {
                request.Number = ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + "Number");
                request.UserDomainID = Utils.ParseGuid(ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + "UserDomainID"));
            });
        }

        #endregion RegisterPublicKeyResponse

        #region ProbeKeyActivationRequest

        /// <summary>
        /// Производит сериализацию запроса на проверку активации ключа.
        /// </summary>
        /// <param name="message">Запрос на проверку активации ключа.</param>
        /// <returns>Сериализованные данные.</returns>
        public string Serialize(ProbeKeyActivationRequest message)
        {
            return CreateMessage(message, (writer, keyRequest) =>
            {
                writer.WriteElementString("KeyNumber", message.KeyNumber);
                writer.WriteElementString("UserDomainID", Utils.GuidToString(message.UserDomainID));
            });
        }

        /// <summary>
        /// Производит десериализацию запроса на проверку активации ключа.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Запрос проверки активации ключа.</returns>
        public ProbeKeyActivationRequest DeserializeProbeKeyActivationRequest(string data)
        {
            return ParseMessage<ProbeKeyActivationRequest>(data, (document, request) =>
            {
                request.KeyNumber = ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + "KeyNumber");
                request.UserDomainID = Utils.ParseGuid(ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + "UserDomainID"));
            });
        }

        #endregion ProbeKeyActivationRequest

        #region ProbeKeyActivationResponse

        /// <summary>
        /// Производит сериализацию ответа с данными по ключу пользователя.
        /// </summary>
        /// <param name="message">Ответ с результатами по ключу пользователя.</param>
        /// <returns>Сериализованные данные.</returns>
        public string Serialize(ProbeKeyActivationResponse message)
        {
            return CreateMessage(message, (writer, keyRequest) =>
            {
                writer.WriteElementString("IsExists", Utils.BooleanToString(message.IsExists));
                writer.WriteElementString("IsExpired", Utils.BooleanToString(message.IsExpired));
                writer.WriteElementString("IsNotAccepted", Utils.BooleanToString(message.IsNotAccepted));
                writer.WriteElementString("IsRevoked", Utils.BooleanToString(message.IsRevoked));
                writer.WriteElementString("UserID", Utils.GuidToString(message.UserID));
            });
        }

        /// <summary>
        /// Производит десериализацию ответа с результатом активации публичного ключа.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Ответ результатом активации публичного ключа.</returns>
        public ProbeKeyActivationResponse DeserializeProbeKeyActivationResponse(string data)
        {
            return ParseMessage<ProbeKeyActivationResponse>(data, (document, request) =>
            {
                request.IsExists = Utils.ParseBoolean(ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + "IsExists"));
                request.IsExpired = Utils.ParseBoolean(ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + "IsExpired"));
                request.IsNotAccepted = Utils.ParseBoolean(ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + "IsNotAccepted"));
                request.IsRevoked = Utils.ParseBoolean(ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + "IsRevoked"));
                request.UserID = Utils.ParseGuid(ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + "UserID"));
            });
        }

        #endregion ProbeKeyActivationResponse

        #region GetDomainUsersRequest

        /// <summary>
        /// Производит сериализацию запроса на проверку активации ключа.
        /// </summary>
        /// <param name="message">Запрос на проверку активации ключа.</param>
        /// <returns>Сериализованные данные.</returns>
        public string Serialize(GetDomainUsersRequest message)
        {
            return CreateMessage(message, (writer, keyRequest) =>
            {
                writer.WriteElementString(XmlConstants.SignData, message.SignData);
                writer.WriteElementString(XmlConstants.UserID, Utils.GuidToString(message.UserID));
            });
        }

        /// <summary>
        /// Производит десериализацию запроса на проверку активации ключа.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Запрос проверки активации ключа.</returns>
        public GetDomainUsersRequest DeserializeGetDomainUsersRequest(string data)
        {
            return ParseMessage<GetDomainUsersRequest>(data, (document, request) =>
            {
                request.SignData = ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + XmlConstants.SignData);
                request.UserID = Utils.ParseGuid(ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + XmlConstants.UserID));
            });
        }

        #endregion GetDomainUsersRequest

        #region GetDomainUsersResponse

        /// <summary>
        /// Производит сериализацию ответа с данными по пользователям домента.
        /// </summary>
        /// <param name="message">Ответ с результатами по пользователям домена.</param>
        /// <returns>Сериализованные данные.</returns>
        public string Serialize(GetDomainUsersResponse message)
        {
            return CreateMessage(message, (writer, keyRequest) =>
            {
                writer.WriteStartElement("Users");

                foreach (var domainUserDTO in message.Users)
                {
                    writer.WriteStartElement("User");
                    WriteAttribute(writer, "UserID", Utils.GuidToString(domainUserDTO.UserID));
                    WriteAttribute(writer, "ProjectRoleID", Utils.IntToString(domainUserDTO.ProjectRoleID));
                    WriteAttribute(writer, "FirstName", domainUserDTO.FirstName);
                    WriteAttribute(writer, "LastName", domainUserDTO.LastName);
                    WriteAttribute(writer, "MiddleName", domainUserDTO.MiddleName);
                    WriteAttribute(writer, "Phone", domainUserDTO.Phone);
                    WriteAttribute(writer, "Email", domainUserDTO.Email);
                    WriteAttribute(writer, "LoginName", domainUserDTO.LoginName);
                    writer.WriteEndElement();
                } //foreach

                writer.WriteEndElement();
            });
        }

        /// <summary>
        /// Производит десериализацию ответа с пользователями домена.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Ответ результатом пользователями домена.</returns>
        public GetDomainUsersResponse DeserializeGetDomainUsersResponse(string data)
        {
            return ParseMessage<GetDomainUsersResponse>(data, (document, response) =>
                                                              {
                                                                  var nodes = document.SelectNodes(XmlConstants.MessageRoot +
                                                                                       "/Users/User");
                                                                  if (nodes!=null)
                                                                  {
                                                                      foreach (XmlNode xmlNode in nodes)
                                                                      {
                                                                          var user = new DomainUserDTO();
                                                                          user.Email = ReadAttribute(xmlNode, "Email");
                                                                          user.LoginName = ReadAttribute(xmlNode, "LoginName");
                                                                          user.FirstName = ReadAttribute(xmlNode, "FirstName");
                                                                          user.LastName = ReadAttribute(xmlNode, "LastName");
                                                                          user.MiddleName = ReadAttribute(xmlNode, "MiddleName");
                                                                          user.Phone = ReadAttribute(xmlNode, "Phone");
                                                                          user.UserID = Utils.ParseGuid(ReadAttribute(xmlNode, "UserID"));
                                                                          user.ProjectRoleID = (byte?)Utils.ParseNullInt(ReadAttribute(xmlNode, "ProjectRoleID"));
                                                                          response.Users.Add(user);
                                                                      } //foreach
                                                                  } //if
                                                              });
        }

        #endregion GetDomainUsersResponse

        #region GetUserBranchesRequest

        /// <summary>
        /// Производит сериализацию запроса на получение данных по филиалам и их связей с пользователями.
        /// </summary>
        /// <param name="message">Запрос на проверку филиалов и их связей с пользователями.</param>
        /// <returns>Сериализованные данные.</returns>
        public string Serialize(GetUserBranchesRequest message)
        {
            return CreateMessage(message, (writer, keyRequest) =>
            {
                writer.WriteElementString(XmlConstants.SignData, message.SignData);
                writer.WriteElementString(XmlConstants.UserID, Utils.GuidToString(message.UserID));
            });
        }

        /// <summary>
        /// Производит десериализацию запроса на получения филиалов и их связей с пользователями.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Запрос проверки филиалов и связей с пользователями.</returns>
        public GetUserBranchesRequest DeserializeGetUserBranchesRequest(string data)
        {
            return ParseMessage<GetUserBranchesRequest>(data, (document, request) =>
            {
                request.SignData = ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + XmlConstants.SignData);
                request.UserID = Utils.ParseGuid(ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + XmlConstants.UserID));
            });
        }

        #endregion GetUserBranchesRequest

        #region GetUserBranchesResponse

        /// <summary>
        /// Производит сериализацию ответа с данными по филиалам и их связям с пользователями.
        /// </summary>
        /// <param name="message">Ответ с результатами по филиалам и их связям с пользователями.</param>
        /// <returns>Сериализованные данные.</returns>
        public string Serialize(GetUserBranchesResponse message)
        {
            return CreateMessage(message, (writer, keyRequest) =>
            {
                writer.WriteStartElement("Branches");

                foreach (var branch in message.Branches)
                {
                    writer.WriteStartElement("Branch");
                    WriteAttribute(writer, "BranchID", Utils.GuidToString(branch.BranchID));
                    WriteAttribute(writer, "LegalName", branch.LegalName);
                    WriteAttribute(writer, "Title", branch.Title);
                    WriteAttribute(writer, "Address", branch.Address);
                    writer.WriteEndElement();
                } //foreach
                writer.WriteEndElement();

                writer.WriteStartElement("UserBranchMapItems");
                foreach (var mapItem in message.UserBranchMapItems)
                {
                    writer.WriteStartElement("UserBranchMapItem");
                    WriteAttribute(writer, "UserBranchMapID", Utils.GuidToString(mapItem.UserBranchMapID));
                    WriteAttribute(writer, "BranchID", Utils.GuidToString(mapItem.BranchID));
                    WriteAttribute(writer, "UserID", Utils.GuidToString(mapItem.UserID));
                    WriteAttribute(writer, "EventDate", Utils.DateTimeToString(mapItem.EventDate));
                    writer.WriteEndElement();
                } //foreach

                writer.WriteEndElement();
            });
        }

        /// <summary>
        /// Производит десериализацию ответа с филиалом и их связям с пользователями.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Ответ результатом филиалов и их связей с пользователями.</returns>
        public GetUserBranchesResponse DeserializeGetUserBranchesResponse(string data)
        {
            return ParseMessage<GetUserBranchesResponse>(data, (document, response) =>
            {
                var nodes = document.SelectNodes(XmlConstants.MessageRoot +
                                     "/Branches/Branch");
                if (nodes != null)
                {
                    foreach (XmlNode xmlNode in nodes)
                    {
                        var branch = new BranchDTO();
                        branch.Address = ReadAttribute(xmlNode, "Address");
                        branch.LegalName = ReadAttribute(xmlNode, "LegalName");
                        branch.Title = ReadAttribute(xmlNode, "Title");
                        branch.BranchID = Utils.ParseGuid(ReadAttribute(xmlNode, "BranchID"));
                        response.Branches.Add(branch);
                    } //foreach
                } //if

                nodes = document.SelectNodes(XmlConstants.MessageRoot +
                                     "/UserBranchMapItems/UserBranchMapItem");
                if (nodes != null)
                {
                    foreach (XmlNode xmlNode in nodes)
                    {
                        var item = new UserBranchMapItemDTO();
                        item.EventDate =Utils.ParseDateTime(ReadAttribute(xmlNode, "EventDate"));
                        item.BranchID = Utils.ParseGuid(ReadAttribute(xmlNode, "BranchID"));
                        item.UserID = Utils.ParseGuid(ReadAttribute(xmlNode, "UserID"));
                        item.UserBranchMapID = Utils.ParseGuid(ReadAttribute(xmlNode, "UserBranchMapID"));
                        response.UserBranchMapItems.Add(item);
                    } //foreach
                } //if
            });
        }

        #endregion GetUserBranchesResponse

        #region GetFinancialGroupBranchesRequest

        /// <summary>
        /// Производит сериализацию запроса на получение данных по финансовым группами и их связей с филиалами.
        /// </summary>
        /// <param name="message">Запрос на получение фингрупп и их связей с филиалами.</param>
        /// <returns>Сериализованные данные.</returns>
        public string Serialize(GetFinancialGroupBranchesRequest message)
        {
            return CreateMessage(message, (writer, keyRequest) =>
            {
                writer.WriteElementString(XmlConstants.SignData, message.SignData);
                writer.WriteElementString(XmlConstants.UserID, Utils.GuidToString(message.UserID));
            });
        }

        /// <summary>
        /// Производит десериализацию запроса на получения фингрупп и их связей с филиалами.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Запрос получения филиалов и связей с филиалами.</returns>
        public GetFinancialGroupBranchesRequest DeserializeGetFinancialGroupBranchesRequest(string data)
        {
            return ParseMessage<GetFinancialGroupBranchesRequest>(data, (document, request) =>
            {
                request.SignData = ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + XmlConstants.SignData);
                request.UserID = Utils.ParseGuid(ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + XmlConstants.UserID));
            });
        }

        #endregion GetFinancialGroupBranchesRequest

        #region GetFinancialGroupBranchesResponse

        /// <summary>
        /// Производит сериализацию ответа с данными по фингруппам и их связям с филиалами.
        /// </summary>
        /// <param name="message">Ответ с результатами по фингруппам и их связям с филиалами.</param>
        /// <returns>Сериализованные данные.</returns>
        public string Serialize(GetFinancialGroupBranchesResponse message)
        {
            return CreateMessage(message, (writer, keyRequest) =>
            {
                writer.WriteStartElement("FinancialGroups");

                foreach (var branch in message.FinancialGroupItems)
                {
                    writer.WriteStartElement("FinancialGroup");
                    WriteAttribute(writer, "FinancialGroupID", Utils.GuidToString(branch.FinancialGroupID));
                    WriteAttribute(writer, "LegalName", branch.LegalName);
                    WriteAttribute(writer, "Title", branch.Title);
                    WriteAttribute(writer, "Trademark", branch.Trademark);
                    writer.WriteEndElement();
                } //foreach
                writer.WriteEndElement();

                writer.WriteStartElement("FinancialGroupBranchMapItems");
                foreach (var mapItem in message.FinancialGroupBranchMapItems)
                {
                    writer.WriteStartElement("FinancialGroupBranchMapItem");
                    WriteAttribute(writer, "FinancialGroupBranchMapID", Utils.GuidToString(mapItem.FinancialGroupBranchMapID));
                    WriteAttribute(writer, "BranchID", Utils.GuidToString(mapItem.BranchID));
                    WriteAttribute(writer, "FinancialGroupID", Utils.GuidToString(mapItem.FinancialGroupID));
                    writer.WriteEndElement();
                } //foreach

                writer.WriteEndElement();
            });
        }

        /// <summary>
        /// Производит десериализацию ответа с фингруппам и их связям с филиалами.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Ответ результатом фингруппам и их связей с филиалами.</returns>
        public GetFinancialGroupBranchesResponse DeserializeGetFinancialGroupBranchesResponse(string data)
        {
            return ParseMessage<GetFinancialGroupBranchesResponse>(data, (document, response) =>
            {
                var nodes = document.SelectNodes(XmlConstants.MessageRoot +
                                     "/FinancialGroups/FinancialGroup");
                if (nodes != null)
                {
                    foreach (XmlNode xmlNode in nodes)
                    {
                        var branch = new FinancialGroupItemDTO();
                        branch.Title = ReadAttribute(xmlNode, "Title");
                        branch.LegalName = ReadAttribute(xmlNode, "LegalName");
                        branch.Trademark = ReadAttribute(xmlNode, "Trademark");
                        branch.FinancialGroupID = Utils.ParseGuid(ReadAttribute(xmlNode, "FinancialGroupID"));
                        response.FinancialGroupItems.Add(branch);
                    } //foreach
                } //if

                nodes = document.SelectNodes(XmlConstants.MessageRoot +
                                     "/FinancialGroupBranchMapItems/FinancialGroupBranchMapItem");
                if (nodes != null)
                {
                    foreach (XmlNode xmlNode in nodes)
                    {
                        var item = new FinancialGroupBranchMapItemDTO();
                        item.BranchID = Utils.ParseGuid(ReadAttribute(xmlNode, "BranchID"));
                        item.FinancialGroupID = Utils.ParseGuid(ReadAttribute(xmlNode, "FinancialGroupID"));
                        item.FinancialGroupBranchMapID = Utils.ParseGuid(ReadAttribute(xmlNode, "FinancialGroupBranchMapID"));
                        response.FinancialGroupBranchMapItems.Add(item);
                    } //foreach
                } //if
            });
        }

        #endregion GetFinancialGroupBranchesResponse

        #region GetWarehousesRequest

        /// <summary>
        /// Производит сериализацию запроса на получение данных по складам и их связей с фингруппами.
        /// </summary>
        /// <param name="message">Запрос на получение складов и их связей с фингруппами.</param>
        /// <returns>Сериализованные данные.</returns>
        public string Serialize(GetWarehousesRequest message)
        {
            return CreateMessage(message, (writer, keyRequest) =>
            {
                writer.WriteElementString(XmlConstants.SignData, message.SignData);
                writer.WriteElementString(XmlConstants.UserID, Utils.GuidToString(message.UserID));
            });
        }

        /// <summary>
        /// Производит десериализацию запроса на получения складов и их связей с фингруппами.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Запрос получения складов и связей с фингруппами.</returns>
        public GetWarehousesRequest DeserializeGetWarehousesRequest(string data)
        {
            return ParseMessage<GetWarehousesRequest>(data, (document, request) =>
            {
                request.SignData = ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + XmlConstants.SignData);
                request.UserID = Utils.ParseGuid(ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + XmlConstants.UserID));
            });
        }

        #endregion GetWarehousesRequest

        #region GetWarehousesResponse

        /// <summary>
        /// Производит сериализацию ответа с данными по складам и их связям с фингруппами.
        /// </summary>
        /// <param name="message">Ответ с результатами по складам и их связям с фингруппами.</param>
        /// <returns>Сериализованные данные.</returns>
        public string Serialize(GetWarehousesResponse message)
        {
            return CreateMessage(message, (writer, keyRequest) =>
            {
                writer.WriteStartElement("Warehouses");

                foreach (var warehouse in message.Warehouses)
                {
                    writer.WriteStartElement("Warehouse");
                    WriteAttribute(writer, "WarehouseID", Utils.GuidToString(warehouse.WarehouseID));
                    WriteAttribute(writer, "Title", warehouse.Title);
                    writer.WriteEndElement();
                } //foreach
                writer.WriteEndElement();

                writer.WriteStartElement("MapItems");
                foreach (var mapItem in message.MapItems)
                {
                    writer.WriteStartElement("MapItem");
                    WriteAttribute(writer, "FinancialGroupWarehouseMapID", Utils.GuidToString(mapItem.FinancialGroupWarehouseMapID));
                    WriteAttribute(writer, "WarehouseID", Utils.GuidToString(mapItem.WarehouseID));
                    WriteAttribute(writer, "FinancialGroupID", Utils.GuidToString(mapItem.FinancialGroupID));
                    writer.WriteEndElement();
                } //foreach

                writer.WriteEndElement();
            });
        }

        /// <summary>
        /// Производит десериализацию ответа со складами и их связям с фингруппами.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Ответ результатом складов и их связей с фингруппами.</returns>
        public GetWarehousesResponse DeserializeGetWarehousesResponse(string data)
        {
            return ParseMessage<GetWarehousesResponse>(data, (document, response) =>
            {
                var nodes = document.SelectNodes(XmlConstants.MessageRoot +
                                     "/Warehouses/Warehouse");
                if (nodes != null)
                {
                    foreach (XmlNode xmlNode in nodes)
                    {
                        var warehouse = new WarehouseDTO();
                        warehouse.Title = ReadAttribute(xmlNode, "Title");
                        warehouse.WarehouseID = Utils.ParseGuid(ReadAttribute(xmlNode, "WarehouseID"));
                        response.Warehouses.Add(warehouse);
                    } //foreach
                } //if

                nodes = document.SelectNodes(XmlConstants.MessageRoot +
                                     "/MapItems/MapItem");
                if (nodes != null)
                {
                    foreach (XmlNode xmlNode in nodes)
                    {
                        var item = new FinancialGroupWarehouseMapItemDTO();
                        item.FinancialGroupWarehouseMapID = Utils.ParseGuid(ReadAttribute(xmlNode, "FinancialGroupWarehouseMapID"));
                        item.FinancialGroupID = Utils.ParseGuid(ReadAttribute(xmlNode, "FinancialGroupID"));
                        item.WarehouseID = Utils.ParseGuid(ReadAttribute(xmlNode, "WarehouseID"));
                        response.MapItems.Add(item);
                    } //foreach
                } //if
            });
        }

        #endregion GetWarehousesResponse

        #region GetGoodsItemRequest

        /// <summary>
        /// Производит сериализацию запроса на получение данных категориям и номенклатурам товаров.
        /// </summary>
        /// <param name="message">Запрос на получение категорий и номенклатур товаров.</param>
        /// <returns>Сериализованные данные.</returns>
        public string Serialize(GetGoodsItemRequest message)
        {
            return CreateMessage(message, (writer, keyRequest) =>
            {
                writer.WriteElementString(XmlConstants.SignData, message.SignData);
                writer.WriteElementString(XmlConstants.UserID, Utils.GuidToString(message.UserID));
            });
        }

        /// <summary>
        /// Производит десериализацию запроса на получения категорий и номенклатур товаров.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Запрос получения категорий и номенклатур товаров.</returns>
        public GetGoodsItemRequest DeserializeGetGoodsItemRequest(string data)
        {
            return ParseMessage<GetGoodsItemRequest>(data, (document, request) =>
            {
                request.SignData = ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + XmlConstants.SignData);
                request.UserID = Utils.ParseGuid(ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + XmlConstants.UserID));
            });
        }

        #endregion GetGoodsItemRequest

        #region GetGoodsItemResponse

        /// <summary>
        /// Производит сериализацию ответа с данными категориям и номенклатурам товаров.
        /// </summary>
        /// <param name="message">Ответ с результатами по категориями и номенклатурам товаров.</param>
        /// <returns>Сериализованные данные.</returns>
        public string Serialize(GetGoodsItemResponse message)
        {
            return CreateMessage(message, (writer, keyRequest) =>
            {
                writer.WriteStartElement("GoodsItems");

                foreach (var goodsItem in message.GoodsItems)
                {
                    writer.WriteStartElement("GoodsItem");
                    WriteAttribute(writer, "GoodsItemID", Utils.GuidToString(goodsItem.GoodsItemID));
                    WriteAttribute(writer, "ItemCategoryID", Utils.GuidToString(goodsItem.ItemCategoryID));
                    WriteAttribute(writer, "DimensionKindID", Utils.IntToString(goodsItem.DimensionKindID));
                    WriteAttribute(writer, "Title", goodsItem.Title);
                    WriteAttribute(writer, "BarCode", goodsItem.BarCode);
                    WriteAttribute(writer, "Description", goodsItem.Description);
                    WriteAttribute(writer, "Particular", goodsItem.Particular);
                    WriteAttribute(writer, "UserCode", goodsItem.UserCode);
                    writer.WriteEndElement();
                } //foreach
                writer.WriteEndElement();

                writer.WriteStartElement("ItemCategories");
                foreach (var itemCategory in message.ItemCategories)
                {
                    writer.WriteStartElement("ItemCategory");
                    WriteAttribute(writer, "ItemCategoryID", Utils.GuidToString(itemCategory.ItemCategoryID));
                    WriteAttribute(writer, "Title", itemCategory.Title);
                    writer.WriteEndElement();
                } //foreach

                writer.WriteEndElement();
            });
        }

        /// <summary>
        /// Производит десериализацию ответа с категориями и номенклатурами товаров.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Ответ результатом номенклатурами и категориями товаров.</returns>
        public GetGoodsItemResponse DeserializeGetGoodsItemResponse(string data)
        {
            return ParseMessage<GetGoodsItemResponse>(data, (document, response) =>
            {
                var nodes = document.SelectNodes(XmlConstants.MessageRoot +
                                     "/GoodsItems/GoodsItem");
                if (nodes != null)
                {
                    foreach (XmlNode xmlNode in nodes)
                    {
                        var goodsItem = new GoodsItemDTO();
                        goodsItem.Title = ReadAttribute(xmlNode, "Title");
                        goodsItem.BarCode = ReadAttribute(xmlNode, "BarCode");
                        goodsItem.Description = ReadAttribute(xmlNode, "Description");
                        goodsItem.Particular = ReadAttribute(xmlNode, "Particular");
                        goodsItem.UserCode = ReadAttribute(xmlNode, "UserCode");
                        goodsItem.GoodsItemID = Utils.ParseGuid(ReadAttribute(xmlNode, "GoodsItemID"));
                        goodsItem.ItemCategoryID = Utils.ParseGuid(ReadAttribute(xmlNode, "ItemCategoryID"));
                        goodsItem.DimensionKindID = (byte?)Utils.ParseNullInt(ReadAttribute(xmlNode, "DimensionKindID"));
                        response.GoodsItems.Add(goodsItem);
                    } //foreach
                } //if

                nodes = document.SelectNodes(XmlConstants.MessageRoot +
                                     "/ItemCategories/ItemCategory");
                if (nodes != null)
                {
                    foreach (XmlNode xmlNode in nodes)
                    {
                        var item = new ItemCategoryDTO();
                        item.ItemCategoryID = Utils.ParseGuid(ReadAttribute(xmlNode, "ItemCategoryID"));
                        item.Title = ReadAttribute(xmlNode, "Title");
                        response.ItemCategories.Add(item);
                    } //foreach
                } //if
            });
        }

        #endregion GetGoodsItemResponse

        #region GetWarehouseItemsRequest

        /// <summary>
        /// Производит сериализацию запроса на получение остатков товаров на складах.
        /// </summary>
        /// <param name="message">Запрос на получение остатков товаров на складах.</param>
        /// <returns>Сериализованные данные.</returns>
        public string Serialize(GetWarehouseItemsRequest message)
        {
            return CreateMessage(message, (writer, keyRequest) =>
            {
                writer.WriteElementString(XmlConstants.SignData, message.SignData);
                writer.WriteElementString(XmlConstants.UserID, Utils.GuidToString(message.UserID));
            });
        }

        /// <summary>
        /// Производит десериализацию запроса на получения остатков товаров на складах.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Запрос получения остатков товаров на складах.</returns>
        public GetWarehouseItemsRequest DeserializeGetWarehouseItemsRequest(string data)
        {
            return ParseMessage<GetWarehouseItemsRequest>(data, (document, request) =>
            {
                request.SignData = ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + XmlConstants.SignData);
                request.UserID = Utils.ParseGuid(ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + XmlConstants.UserID));
            });
        }

        #endregion GetWarehouseItemsRequest

        #region GetWarehouseItemsResponse

        /// <summary>
        /// Производит сериализацию ответа с остатками товаров на складах.
        /// </summary>
        /// <param name="message">Ответ с результатами по остатков товаров на складах.</param>
        /// <returns>Сериализованные данные.</returns>
        public string Serialize(GetWarehouseItemsResponse message)
        {
            return CreateMessage(message, (writer, keyRequest) =>
            {
                writer.WriteStartElement("WarehouseItems");

                foreach (var warehouseItem in message.WarehouseItems)
                {
                    writer.WriteStartElement("WarehouseItem");
                    WriteAttribute(writer, "WarehouseItemID", Utils.GuidToString(warehouseItem.WarehouseItemID));
                    WriteAttribute(writer, "GoodsItemID", Utils.GuidToString(warehouseItem.GoodsItemID));
                    WriteAttribute(writer, "WarehouseID", Utils.GuidToString(warehouseItem.WarehouseID));
                    WriteAttribute(writer, "RepairPrice", Utils.DecimalToString(warehouseItem.RepairPrice));
                    WriteAttribute(writer, "Total", Utils.DecimalToString(warehouseItem.Total));
                    WriteAttribute(writer, "SalePrice", Utils.DecimalToString(warehouseItem.SalePrice));
                    WriteAttribute(writer, "StartPrice", Utils.DecimalToString(warehouseItem.StartPrice));
                    
                    writer.WriteEndElement();
                } //foreach
                writer.WriteEndElement();

            });
        }

        /// <summary>
        /// Производит десериализацию ответа с категориями и номенклатурами товаров.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Ответ результатом номенклатурами и категориями товаров.</returns>
        public GetWarehouseItemsResponse DeserializeGetWarehouseItemsResponse(string data)
        {
            return ParseMessage<GetWarehouseItemsResponse>(data, (document, response) =>
            {
                var nodes = document.SelectNodes(XmlConstants.MessageRoot +
                                     "/WarehouseItems/WarehouseItem");
                if (nodes != null)
                {
                    foreach (XmlNode xmlNode in nodes)
                    {
                        var warehouseItem = new WarehouseItemDTO();
                        warehouseItem.RepairPrice = Utils.GetDecimalValueOrNull(ReadAttribute(xmlNode, "RepairPrice")) ?? decimal.Zero;
                        warehouseItem.SalePrice = Utils.GetDecimalValueOrNull(ReadAttribute(xmlNode, "SalePrice")) ?? decimal.Zero;
                        warehouseItem.StartPrice = Utils.GetDecimalValueOrNull(ReadAttribute(xmlNode, "StartPrice")) ?? decimal.Zero;
                        warehouseItem.Total = Utils.GetDecimalValueOrNull(ReadAttribute(xmlNode, "Total")) ?? decimal.Zero;
                        
                        warehouseItem.WarehouseItemID = Utils.ParseGuid(ReadAttribute(xmlNode, "WarehouseItemID"));
                        warehouseItem.WarehouseID = Utils.ParseGuid(ReadAttribute(xmlNode, "WarehouseID"));
                        warehouseItem.GoodsItemID = Utils.ParseGuid(ReadAttribute(xmlNode, "GoodsItemID"));
                        
                        response.WarehouseItems.Add(warehouseItem);
                    } //foreach
                } //if
            });
        }

        #endregion GetWarehouseItemsResponse

        #region GetOrderStatusesRequest

        /// <summary>
        /// Производит сериализацию запроса на получение статусов заказов.
        /// </summary>
        /// <param name="message">Запрос на получение статусов заказов.</param>
        /// <returns>Сериализованные данные.</returns>
        public string Serialize(GetOrderStatusesRequest message)
        {
            return CreateMessage(message, (writer, keyRequest) =>
            {
                writer.WriteElementString(XmlConstants.SignData, message.SignData);
                writer.WriteElementString(XmlConstants.UserID, Utils.GuidToString(message.UserID));
            });
        }

        /// <summary>
        /// Производит десериализацию запроса на получение статусов заказов
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Запрос получения статусов заказов.</returns>
        public GetOrderStatusesRequest DeserializeGetOrderStatusesRequest(string data)
        {
            return ParseMessage<GetOrderStatusesRequest>(data, (document, request) =>
            {
                request.SignData = ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + XmlConstants.SignData);
                request.UserID = Utils.ParseGuid(ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + XmlConstants.UserID));
            });
        }

        #endregion GetOrderStatusesRequest

        #region GetOrderStatusesResponse

        /// <summary>
        /// Производит сериализацию ответа с данными по статусам заказов.
        /// </summary>
        /// <param name="message">Ответ с результатами по статусам заказов.</param>
        /// <returns>Сериализованные данные.</returns>
        public string Serialize(GetOrderStatusesResponse message)
        {
            return CreateMessage(message, (writer, keyRequest) =>
            {
                writer.WriteStartElement("OrderKinds");

                foreach (var orderKind in message.OrderKinds)
                {
                    writer.WriteStartElement("OrderKind");
                    WriteAttribute(writer, "OrderKindID", Utils.GuidToString(orderKind.OrderKindID));
                    WriteAttribute(writer, "Title", orderKind.Title);
                    writer.WriteEndElement();
                } //foreach
                writer.WriteEndElement();

                writer.WriteStartElement("OrderStatuses");
                foreach (var orderStatus in message.OrderStatuses)
                {
                    writer.WriteStartElement("OrderStatus");
                    WriteAttribute(writer, "OrderStatusID", Utils.GuidToString(orderStatus.OrderStatusID));
                    WriteAttribute(writer, "StatusKindID", Utils.IntToString(orderStatus.StatusKindID));
                    WriteAttribute(writer, "Title", orderStatus.Title);
                    writer.WriteEndElement();
                } //foreach

                writer.WriteEndElement();
            });
        }

        /// <summary>
        /// Производит десериализацию ответа со статусами заказов.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Ответ результатом по статусам заказов.</returns>
        public GetOrderStatusesResponse DeserializeGetOrderStatusesResponse(string data)
        {
            return ParseMessage<GetOrderStatusesResponse>(data, (document, response) =>
            {
                var nodes = document.SelectNodes(XmlConstants.MessageRoot +
                                     "/OrderKinds/OrderKind");
                if (nodes != null)
                {
                    foreach (XmlNode xmlNode in nodes)
                    {
                        var orderKind = new OrderKindDTO();
                        orderKind.Title = ReadAttribute(xmlNode, "Title");
                        orderKind.OrderKindID = Utils.ParseGuid(ReadAttribute(xmlNode, "OrderKindID"));
                        response.OrderKinds.Add(orderKind);
                    } //foreach
                } //if

                nodes = document.SelectNodes(XmlConstants.MessageRoot +
                                     "/OrderStatuses/OrderStatus");
                if (nodes != null)
                {
                    foreach (XmlNode xmlNode in nodes)
                    {
                        var item = new OrderStatusDTO();
                        item.OrderStatusID = Utils.ParseGuid(ReadAttribute(xmlNode, "OrderStatusID"));
                        item.StatusKindID = (byte?)Utils.ParseInt(ReadAttribute(xmlNode, "StatusKindID"));
                        item.Title = ReadAttribute(xmlNode, "Title");
                        response.OrderStatuses.Add(item);
                    } //foreach
                } //if
            });
        }

        #endregion GetOrderStatusesResponse

        #region GetServerRepairOrderHashesRequest

        /// <summary>
        /// Производит сериализацию запроса на получение серверных хэшей заказов.
        /// </summary>
        /// <param name="message">Запрос на получение серверных хэшей заказов.</param>
        /// <returns>Сериализованные данные.</returns>
        public string Serialize(GetServerRepairOrderHashesRequest message)
        {
            return CreateMessage(message, (writer, keyRequest) =>
            {
                writer.WriteElementString(XmlConstants.SignData, message.SignData);
                writer.WriteElementString(XmlConstants.UserID, Utils.GuidToString(message.UserID));
                writer.WriteElementString("LastRepairOrderID", Utils.GuidToString(message.LastRepairOrderID));
            });
        }

        /// <summary>
        /// Производит десериализацию запроса на получение серверных хэшей заказов
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Запрос получения серверных хэшей заказов.</returns>
        public GetServerRepairOrderHashesRequest DeserializeGetServerRepairOrderHashesRequest(string data)
        {
            return ParseMessage<GetServerRepairOrderHashesRequest>(data, (document, request) =>
            {
                request.SignData = ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + XmlConstants.SignData);
                request.UserID = Utils.ParseGuid(ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + XmlConstants.UserID));
                request.LastRepairOrderID = Utils.ParseGuid(ReadRequiredElement(document, XmlConstants.MessageRoot + "/LastRepairOrderID"));
            });
        }

        #endregion GetServerRepairOrderHashesRequest

        #region GetServerRepairOrderHashesResponse

        /// <summary>
        /// Производит сериализацию ответа с серверными хэшами заказов.
        /// </summary>
        /// <param name="message">Ответ с серверными хэшами заказов.</param>
        /// <returns>Сериализованные данные.</returns>
        public string Serialize(GetServerRepairOrderHashesResponse message)
        {
            return CreateMessage(message, (writer, keyRequest) =>
            {
                writer.WriteElementString("TotalCount",Utils.IntToString(message.TotalCount));
                writer.WriteStartElement("RepairOrderServerHashes");

                foreach (var repairOrderServerHash in message.RepairOrderServerHashes)
                {
                    writer.WriteStartElement("RepairOrderServerHash");
                    WriteAttribute(writer, "RepairOrderID", Utils.GuidToString(repairOrderServerHash.RepairOrderID));
                    WriteAttribute(writer, "OrderTimelinesCount", Utils.IntToString(repairOrderServerHash.OrderTimelinesCount));
                    WriteAttribute(writer, "DataHash", repairOrderServerHash.DataHash);

                    writer.WriteStartElement("DeviceItems");

                    foreach (var deviceItemServerHashDTO in repairOrderServerHash.DeviceItems)
                    {
                        writer.WriteStartElement("DeviceItem");

                        WriteAttribute(writer, "DeviceItemID", Utils.GuidToString(deviceItemServerHashDTO.DeviceItemID));
                        WriteAttribute(writer, "DataHash", deviceItemServerHashDTO.DataHash);
                        
                        writer.WriteEndElement();
                    } //foreach

                    writer.WriteEndElement();

                    writer.WriteStartElement("WorkItems");

                    foreach (var workItemServerHashDTO in repairOrderServerHash.WorkItems)
                    {
                        writer.WriteStartElement("WorkItem");

                        WriteAttribute(writer, "WorkItemID", Utils.GuidToString(workItemServerHashDTO.WorkItemID));
                        WriteAttribute(writer, "DataHash", workItemServerHashDTO.DataHash);

                        writer.WriteEndElement();
                    } //foreach

                    writer.WriteEndElement();


                    writer.WriteEndElement();
                } //foreach
                writer.WriteEndElement();

            });
        }

        /// <summary>
        /// Производит десериализацию ответа с серверными хэшами заказов.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Ответ серверными хэшами заказов.</returns>
        public GetServerRepairOrderHashesResponse DeserializeGetServerRepairOrderHashesResponse(string data)
        {
            return ParseMessage<GetServerRepairOrderHashesResponse>(data, (document, response) =>
            {
                var node = document.SelectSingleNode(XmlConstants.MessageRoot + "/TotalCount");

                if (node!=null)
                {
                    response.TotalCount = Utils.ParseInt(node.InnerText);
                } //if

                var nodes = document.SelectNodes(XmlConstants.MessageRoot +
                                     "/RepairOrderServerHashes/RepairOrderServerHash");
                if (nodes != null)
                {
                    foreach (XmlNode xmlNode in nodes)
                    {
                        var orderServerHash = new RepairOrderServerHashDTO();
                        orderServerHash.DataHash = ReadAttribute(xmlNode, "DataHash");
                        orderServerHash.OrderTimelinesCount = Utils.GetLongOrNull(ReadAttribute(xmlNode, "OrderTimelinesCount"))??0;
                        orderServerHash.RepairOrderID = Utils.ParseGuid(ReadAttribute(xmlNode, "RepairOrderID"));

                        var workNodes = xmlNode.SelectNodes("WorkItems/WorkItem");

                        if (workNodes != null)
                        {
                            foreach (XmlNode workNode in workNodes)
                            {
                                var workItem = new WorkItemServerHashDTO();
                                workItem.DataHash = ReadAttribute(workNode, "DataHash");
                                workItem.WorkItemID = Utils.ParseGuid(ReadAttribute(workNode, "WorkItemID"));
                                orderServerHash.WorkItems.Add(workItem);
                            } //foreach
                        } //if

                        var deviceNodes = xmlNode.SelectNodes("DeviceItems/DeviceItem");

                        if (deviceNodes != null)
                        {
                            foreach (XmlNode deviceNode in deviceNodes)
                            {
                                var deviceItem = new DeviceItemServerHashDTO();
                                deviceItem.DataHash = ReadAttribute(deviceNode, "DataHash");
                                deviceItem.DeviceItemID = Utils.ParseGuid(ReadAttribute(deviceNode, "DeviceItemID"));
                                orderServerHash.DeviceItems.Add(deviceItem);
                            } //foreach
                        } //if

                        response.RepairOrderServerHashes.Add(orderServerHash);
                    } //foreach
                } //if
            });
        }

        #endregion GetServerRepairOrderHashesResponse

        #region GetRepairOrdersRequest

        /// <summary>
        /// Производит сериализацию запроса на получение серверных заказов.
        /// </summary>
        /// <param name="message">Запрос на получение серверных заказов.</param>
        /// <returns>Сериализованные данные.</returns>
        public string Serialize(GetRepairOrdersRequest message)
        {
            return CreateMessage(message, (writer, keyRequest) =>
            {
                writer.WriteElementString(XmlConstants.SignData, message.SignData);
                writer.WriteElementString(XmlConstants.UserID, Utils.GuidToString(message.UserID));
                writer.WriteStartElement("RepairOrderIds");

                foreach (var repairOrderId in message.RepairOrderIds)
                {
                    writer.WriteElementString("Id", Utils.GuidToString(repairOrderId));
                } //foreach

                writer.WriteEndElement();
                
            });
        }

        /// <summary>
        /// Производит десериализацию запроса на получение серверных хэшей заказов
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Запрос получения серверных хэшей заказов.</returns>
        public GetRepairOrdersRequest DeserializeGetRepairOrdersRequest(string data)
        {
            return ParseMessage<GetRepairOrdersRequest>(data, (document, request) =>
            {
                request.SignData = ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + XmlConstants.SignData);
                request.UserID = Utils.ParseGuid(ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + XmlConstants.UserID));

                var nodes = document.SelectNodes(XmlConstants.MessageRoot + "/RepairOrderIds/Id");

                if (nodes!=null)
                {
                    foreach (XmlNode xmlNode in nodes)
                    {
                        request.RepairOrderIds.Add(Utils.ParseGuid(xmlNode.InnerText));
                    } //foreach
                } //if

            });
        }

        #endregion GetRepairOrdersRequest

        #region GetRepairOrdersResponse

        /// <summary>
        /// Производит сериализацию ответа с серверными заказами.
        /// </summary>
        /// <param name="message">Ответ с серверными заказами.</param>
        /// <returns>Сериализованные данные.</returns>
        public string Serialize(GetRepairOrdersResponse message)
        {
            return CreateMessage(message, (writer, keyRequest) =>
            {
                writer.WriteStartElement("RepairOrders");

                foreach (var repairOrder in message.RepairOrders)
                {
                    WriteRepairOrder(writer, repairOrder);
                } //foreach
                writer.WriteEndElement();

            });
        }

        /// <summary>
        /// Записывает в xml представление заказа.
        /// </summary>
        /// <param name="writer">Xml writer.</param>
        /// <param name="repairOrder">Записываемый объект заказа.</param>
        private void WriteRepairOrder(XmlWriter writer, RepairOrderDTO repairOrder)
        {
            writer.WriteStartElement("RepairOrder");
            WriteAttribute(writer, "RepairOrderID", Utils.GuidToString(repairOrder.RepairOrderID));
            WriteAttribute(writer, "BranchID", Utils.GuidToString(repairOrder.BranchID));
            WriteAttribute(writer, "CallEventDate", Utils.DateTimeToString(repairOrder.CallEventDate));
            WriteAttribute(writer, "ClientAddress", repairOrder.ClientAddress);
            WriteAttribute(writer, "ClientEmail", repairOrder.ClientEmail);
            WriteAttribute(writer, "ClientFullName", repairOrder.ClientFullName);
            WriteAttribute(writer, "ClientPhone", repairOrder.ClientPhone);
            WriteAttribute(writer, "DateOfBeReady", Utils.DateTimeToString(repairOrder.DateOfBeReady));
            WriteAttribute(writer, "Defect", repairOrder.Defect);
            WriteAttribute(writer, "DeviceAppearance", repairOrder.DeviceAppearance);
            WriteAttribute(writer, "DeviceModel", repairOrder.DeviceModel);
            WriteAttribute(writer, "DeviceSN", repairOrder.DeviceSN);
            WriteAttribute(writer, "DeviceTitle", repairOrder.DeviceTitle);
            WriteAttribute(writer, "DeviceTrademark", repairOrder.DeviceTrademark);
            WriteAttribute(writer, "EngineerID", Utils.GuidToString(repairOrder.EngineerID));
            WriteAttribute(writer, "EventDate", Utils.DateTimeToString(repairOrder.EventDate));
            WriteAttribute(writer, "GuidePrice", Utils.DecimalToString(repairOrder.GuidePrice));
            WriteAttribute(writer, "IsUrgent", Utils.BooleanToString(repairOrder.IsUrgent));
            WriteAttribute(writer, "IssueDate", Utils.DateTimeToString(repairOrder.IssueDate));
            WriteAttribute(writer, "IssuerID", Utils.GuidToString(repairOrder.IssuerID));
            WriteAttribute(writer, "ManagerID", Utils.GuidToString(repairOrder.ManagerID));
            WriteAttribute(writer, "Notes", repairOrder.Notes);
            WriteAttribute(writer, "Number", repairOrder.Number);
            WriteAttribute(writer, "Options", repairOrder.Options);
            WriteAttribute(writer, "OrderKindID", Utils.GuidToString(repairOrder.OrderKindID));
            WriteAttribute(writer, "OrderStatusID", Utils.GuidToString(repairOrder.OrderStatusID));
            WriteAttribute(writer, "PrePayment", Utils.DecimalToString(repairOrder.PrePayment));
            WriteAttribute(writer, "Recommendation", repairOrder.Recommendation);
            WriteAttribute(writer, "WarrantyTo", Utils.DateTimeToString(repairOrder.WarrantyTo));

            writer.WriteStartElement("DeviceItems");

            foreach (var item in repairOrder.DeviceItems)
            {
                writer.WriteStartElement("DeviceItem");

                WriteAttribute(writer, "DeviceItemID", Utils.GuidToString(item.DeviceItemID));
                WriteAttribute(writer, "CostPrice", Utils.DecimalToString(item.CostPrice));
                WriteAttribute(writer, "Count", Utils.DecimalToString(item.Count));
                WriteAttribute(writer, "EventDate", Utils.DateTimeToString(item.EventDate));
                WriteAttribute(writer, "Price", Utils.DecimalToString(item.Price));
                WriteAttribute(writer, "RepairOrderID", Utils.GuidToString(item.RepairOrderID));
                WriteAttribute(writer, "UserID", Utils.GuidToString(item.UserID));
                WriteAttribute(writer, "WarehouseItemID", Utils.GuidToString(item.WarehouseItemID));
                WriteAttribute(writer, "Title", item.Title);

                writer.WriteEndElement();
            } //foreach

            writer.WriteEndElement();

            writer.WriteStartElement("WorkItems");

            foreach (var item in repairOrder.WorkItems)
            {
                writer.WriteStartElement("WorkItem");

                WriteAttribute(writer, "WorkItemID", Utils.GuidToString(item.WorkItemID));
                WriteAttribute(writer, "RepairOrderID", Utils.GuidToString(item.RepairOrderID));
                WriteAttribute(writer, "UserID", Utils.GuidToString(item.UserID));
                WriteAttribute(writer, "Title", item.Title);
                WriteAttribute(writer, "EventDate", Utils.DateTimeToString(item.EventDate));
                WriteAttribute(writer, "Price", Utils.DecimalToString(item.Price));

                writer.WriteEndElement();
            } //foreach

            writer.WriteEndElement();

            writer.WriteStartElement("OrderTimelines");

            foreach (var timeline in repairOrder.OrderTimelines)
            {
                writer.WriteStartElement("OrderTimeline");

                WriteAttribute(writer, "OrderTimelineID", Utils.GuidToString(timeline.OrderTimelineID));
                WriteAttribute(writer, "EventDateTime", Utils.DateTimeToString(timeline.EventDateTime));
                WriteAttribute(writer, "RepairOrderID", Utils.GuidToString(timeline.RepairOrderID));
                WriteAttribute(writer, "Title", timeline.Title);
                WriteAttribute(writer, "TimelineKindID", Utils.IntToString(timeline.TimelineKindID));

                writer.WriteEndElement();
            } //foreach

            writer.WriteEndElement();

            writer.WriteEndElement();
        }

        /// <summary>
        /// Производит десериализацию ответа с серверными заказами.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Ответ серверными заказами.</returns>
        public GetRepairOrdersResponse DeserializeGetRepairOrdersResponse(string data)
        {
            return ParseMessage<GetRepairOrdersResponse>(data, (document, response) =>
            {
                
                var nodes = document.SelectNodes(XmlConstants.MessageRoot +
                                     "/RepairOrders/RepairOrder");
                if (nodes != null)
                {
                    foreach (XmlNode xmlNode in nodes)
                    {
                        var repairOrder = ReadRepairOrder(xmlNode);

                        response.RepairOrders.Add(repairOrder);
                    } //foreach
                } //if
            });
        }

        /// <summary>
        /// Считывает Xml представление объекта заказа.
        /// </summary>
        /// <param name="xmlNode">Xml нод объекта.</param>
        /// <returns>Созданный объект.</returns>
        private RepairOrderDTO ReadRepairOrder(XmlNode xmlNode)
        {
            var repairOrder = new RepairOrderDTO();
            repairOrder.RepairOrderID = Utils.ParseGuid(ReadAttribute(xmlNode, "RepairOrderID"));
            repairOrder.BranchID = Utils.ParseGuid(ReadAttribute(xmlNode, "BranchID"));
            repairOrder.CallEventDate = Utils.ParseDateTime(ReadAttribute(xmlNode, "CallEventDate"));
            repairOrder.ClientAddress = ReadAttribute(xmlNode, "ClientAddress");
            repairOrder.ClientEmail = ReadAttribute(xmlNode, "ClientEmail");
            repairOrder.ClientFullName = ReadAttribute(xmlNode, "ClientFullName");
            repairOrder.ClientPhone = ReadAttribute(xmlNode, "ClientPhone");
            repairOrder.Defect = ReadAttribute(xmlNode, "Defect");
            repairOrder.DeviceAppearance = ReadAttribute(xmlNode, "DeviceAppearance");
            repairOrder.DeviceModel = ReadAttribute(xmlNode, "DeviceModel");
            repairOrder.DeviceSN = ReadAttribute(xmlNode, "DeviceSN");
            repairOrder.DeviceTitle = ReadAttribute(xmlNode, "DeviceTitle");
            repairOrder.DeviceTrademark = ReadAttribute(xmlNode, "DeviceTrademark");
            repairOrder.Notes = ReadAttribute(xmlNode, "Notes");
            repairOrder.Number = ReadAttribute(xmlNode, "Number");
            repairOrder.Options = ReadAttribute(xmlNode, "Options");
            repairOrder.Recommendation = ReadAttribute(xmlNode, "Recommendation");
            repairOrder.DateOfBeReady = Utils.ParseDateTime(ReadAttribute(xmlNode, "DateOfBeReady")) ?? DateTime.MinValue;
            repairOrder.EngineerID = Utils.ParseGuid(ReadAttribute(xmlNode, "EngineerID"));
            repairOrder.EventDate = Utils.ParseDateTime(ReadAttribute(xmlNode, "EventDate")) ?? DateTime.MaxValue;
            repairOrder.GuidePrice = Utils.GetDecimalValueOrNull(ReadAttribute(xmlNode, "GuidePrice"));
            repairOrder.IsUrgent = Utils.ParseBoolean(ReadAttribute(xmlNode, "IsUrgent"));
            repairOrder.IssueDate = Utils.ParseDateTime(ReadAttribute(xmlNode, "IssueDate"));
            repairOrder.IssuerID = Utils.ParseGuid(ReadAttribute(xmlNode, "IssuerID"));
            repairOrder.ManagerID = Utils.ParseGuid(ReadAttribute(xmlNode, "ManagerID"));
            repairOrder.OrderKindID = Utils.ParseGuid(ReadAttribute(xmlNode, "OrderKindID"));
            repairOrder.OrderStatusID = Utils.ParseGuid(ReadAttribute(xmlNode, "OrderStatusID"));
            repairOrder.PrePayment = Utils.GetDecimalValueOrNull(ReadAttribute(xmlNode, "PrePayment"));
            repairOrder.WarrantyTo = Utils.ParseDateTime(ReadAttribute(xmlNode, "WarrantyTo"));

            var workNodes = xmlNode.SelectNodes("WorkItems/WorkItem");

            if (workNodes != null)
            {
                foreach (XmlNode workNode in workNodes)
                {
                    var workItem = new WorkItemDTO();
                    workItem.EventDate = Utils.ParseDateTime(ReadAttribute(workNode, "EventDate")) ?? DateTime.MinValue;
                    workItem.WorkItemID = Utils.ParseGuid(ReadAttribute(workNode, "WorkItemID"));
                    workItem.RepairOrderID = Utils.ParseGuid(ReadAttribute(workNode, "RepairOrderID"));
                    workItem.UserID = Utils.ParseGuid(ReadAttribute(workNode, "UserID"));
                    workItem.Title = ReadAttribute(workNode, "Title");
                    workItem.Price = Utils.GetDecimalValueOrNull(ReadAttribute(workNode, "Price"))??decimal.Zero;
                    repairOrder.WorkItems.Add(workItem);
                } //foreach
            } //if

            var deviceNodes = xmlNode.SelectNodes("DeviceItems/DeviceItem");

            if (deviceNodes != null)
            {
                foreach (XmlNode deviceNode in deviceNodes)
                {
                    var deviceItem = new DeviceItemDTO();
                    deviceItem.CostPrice = Utils.GetDecimalValueOrNull(ReadAttribute(deviceNode, "CostPrice"))??decimal.Zero;
                    deviceItem.Price = Utils.GetDecimalValueOrNull(ReadAttribute(deviceNode, "Price")) ?? decimal.Zero;
                    deviceItem.DeviceItemID = Utils.ParseGuid(ReadAttribute(deviceNode, "DeviceItemID"));
                    deviceItem.RepairOrderID = Utils.ParseGuid(ReadAttribute(deviceNode, "RepairOrderID"));
                    deviceItem.WarehouseItemID = Utils.ParseGuid(ReadAttribute(deviceNode, "WarehouseItemID"));
                    deviceItem.UserID = Utils.ParseGuid(ReadAttribute(deviceNode, "UserID"));
                    deviceItem.Title = ReadAttribute(deviceNode, "Title");
                    deviceItem.Count = Utils.GetDecimalValueOrNull(ReadAttribute(deviceNode, "Count")) ?? decimal.Zero;
                    deviceItem.EventDate = Utils.ParseDateTime(ReadAttribute(deviceNode, "EventDate")) ?? DateTime.MinValue;
                    repairOrder.DeviceItems.Add(deviceItem);
                } //foreach
            } //if

            var timelineNodes = xmlNode.SelectNodes("OrderTimelines/OrderTimeline");

            if (timelineNodes != null)
            {
                foreach (XmlNode deviceNode in timelineNodes)
                {
                    var deviceItem = new OrderTimelineDTO();

                    deviceItem.OrderTimelineID = Utils.ParseGuid(ReadAttribute(deviceNode, "OrderTimelineID"));
                    deviceItem.RepairOrderID = Utils.ParseGuid(ReadAttribute(deviceNode, "RepairOrderID"));
                    deviceItem.Title = ReadAttribute(deviceNode, "Title");
                    deviceItem.TimelineKindID = Utils.ParseByte(ReadAttribute(deviceNode, "TimelineKindID"));

                    deviceItem.EventDateTime = Utils.ParseDateTime(ReadAttribute(deviceNode, "EventDateTime")) ??
                                               DateTime.MinValue;
                    repairOrder.OrderTimelines.Add(deviceItem);
                } //foreach
            } //if
            return repairOrder;
        }

        #endregion GetRepairOrdersResponse

        #region SaveRepairOrderRequest

        /// <summary>
        /// Производит сериализацию запроса на сохранение заказа.
        /// </summary>
        /// <param name="message">Запрос на сохранение заказа</param>
        /// <returns>Сериализованные данные.</returns>
        public string Serialize(SaveRepairOrderRequest message)
        {
            return CreateMessage(message, (writer, keyRequest) =>
            {
                writer.WriteElementString(XmlConstants.SignData, message.SignData);
                writer.WriteElementString(XmlConstants.UserID, Utils.GuidToString(message.UserID));

                WriteRepairOrder(writer, message.RepairOrder);

            });
        }

        /// <summary>
        /// Производит десериализацию запроса на сохранение заказа.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Запрос сохранение заказа на сервере.</returns>
        public SaveRepairOrderRequest DeserializeSaveRepairOrderRequest(string data)
        {
            return ParseMessage<SaveRepairOrderRequest>(data, (document, request) =>
            {
                request.SignData = ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + XmlConstants.SignData);
                request.UserID = Utils.ParseGuid(ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + XmlConstants.UserID));

                var node = document.SelectSingleNode(XmlConstants.MessageRoot + "/RepairOrder");

                if (node != null)
                {
                    request.RepairOrder = ReadRepairOrder(node);
                } //if

            });
        }

        #endregion SaveRepairOrderRequest

        #region SaveRepairOrderResponse

        /// <summary>
        /// Производит сериализацию ответа на сохранение заказа.
        /// </summary>
        /// <param name="message">Ответ с результатом сохранения заказов.</param>
        /// <returns>Сериализованные данные.</returns>
        public string Serialize(SaveRepairOrderResponse message)
        {
            return CreateMessage(message, (writer, keyRequest) => writer.WriteElementString("Success", Utils.BooleanToString(message.Success)));
        }

        /// <summary>
        /// Производит десериализацию ответа с результатом сохранения заказов.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Ответ результатом сохранения заказа.</returns>
        public SaveRepairOrderResponse DeserializeSaveRepairOrderResponse(string data)
        {
            return ParseMessage<SaveRepairOrderResponse>(data, (document, response) =>
            {
                var node = document.SelectSingleNode(XmlConstants.MessageRoot + "/Success");

                if (node != null)
                {
                    response.Success = Utils.ParseBoolean(node.InnerText);
                } //if
            });
        }

        #endregion SaveRepairOrderResponse

        #region GetCustomReportItemRequest

        /// <summary>
        /// Производит сериализацию запроса на получение пользовательских отчетов.
        /// </summary>
        /// <param name="message">Запрос на получение пользовательских отчетов.</param>
        /// <returns>Сериализованные данные.</returns>
        public string Serialize(GetCustomReportItemRequest message)
        {
            return CreateMessage(message, (writer, keyRequest) =>
            {
                writer.WriteElementString(XmlConstants.SignData, message.SignData);
                writer.WriteElementString(XmlConstants.UserID, Utils.GuidToString(message.UserID));
            });
        }

        /// <summary>
        /// Производит десериализацию запроса на получение пользовательских отчетов.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Запрос получение пользовательских отчетов.</returns>
        public GetCustomReportItemRequest DeserializeGetCustomReportItemRequest(string data)
        {
            return ParseMessage<GetCustomReportItemRequest>(data, (document, request) =>
            {
                request.SignData = ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + XmlConstants.SignData);
                request.UserID = Utils.ParseGuid(ReadRequiredElement(document, XmlConstants.MessageRoot + "/" + XmlConstants.UserID));
            });
        }

        #endregion GetCustomReportItemRequest

        #region GetCustomReportItemResponse

        /// <summary>
        /// Производит сериализацию ответа с данными по пользовательским отчетам.
        /// </summary>
        /// <param name="message">Ответ с результатами по пользовательским отчетам.</param>
        /// <returns>Сериализованные данные.</returns>
        public string Serialize(GetCustomReportItemResponse message)
        {
            return CreateMessage(message, (writer, keyRequest) =>
                                          {
                                              writer.WriteStartElement("CustomReportItems");

                                              foreach (var reportItemDTO in message.CustomReportItems)
                                              {
                                                  writer.WriteStartElement("CustomReportItem");
                                                  WriteAttribute(writer, "CustomReportID",
                                                                 Utils.GuidToString(reportItemDTO.CustomReportID));
                                                  WriteAttribute(writer, "DocumentKindID",
                                                                 Utils.IntToString(reportItemDTO.DocumentKindID));
                                                  WriteAttribute(writer, "Title", reportItemDTO.Title);
                                                  writer.WriteStartElement("HtmlContent");
                                                  writer.WriteCData(reportItemDTO.HtmlContent);
                                                  writer.WriteEndElement();
                                                  writer.WriteEndElement();
                                              } //foreach

                                              writer.WriteEndElement();
                                          });
        }

        /// <summary>
        /// Производит десериализацию ответа с пользовательскими отчетами.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Ответ результатом пользовательских отчетов.</returns>
        public GetCustomReportItemResponse DeserializeGetCustomReportItemResponse(string data)
        {
            return ParseMessage<GetCustomReportItemResponse>(data, (document, response) =>
                                                              {
                                                                  var nodes =
                                                                      document.SelectNodes(XmlConstants.MessageRoot +
                                                                                           "/CustomReportItems/CustomReportItem");
                                                                  if (nodes != null)
                                                                  {
                                                                      foreach (XmlNode xmlNode in nodes)
                                                                      {
                                                                          var report = new CustomReportItemDTO();
                                                                          report.Title = ReadAttribute(xmlNode, "Title");
                                                                          
                                                                          report.DocumentKindID = (byte)Utils.ParseInt(ReadAttribute(xmlNode, "DocumentKindID"));
                                                                          report.CustomReportID =
                                                                              Utils.ParseGuid(ReadAttribute(xmlNode,
                                                                                                            "CustomReportID"));
                                                                          report.HtmlContent =
                                                                              ReadRequiredElement(xmlNode, "HtmlContent");

                                                                          response.CustomReportItems.Add(report);
                                                                      } //foreach
                                                                  } //if
                                                              });
        }

        #endregion GetCustomReportItemResponse
    }
}
