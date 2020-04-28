using Exemplo.Identidade.API.Tests.Config;
using Exemplo.Identidade.API.ViewModels;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace Exemplo.Identidade.API.Tests
{
    [TestCaseOrderer("Exemplo.Identidade.API.Tests.Config.PriorityOrderer", "Exemplo.Identidade.API.Tests")]
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class UsuarioTests
    {
        private readonly IntegrationTestsFixture<StartupApiTests> _testsFixture;
        public UsuarioTests(IntegrationTestsFixture<StartupApiTests> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Realizar cadastro senha fraca")]
        [Trait("Categoria", "Integração API - Usuário")]
        public async void Usuario_RealizarCadastroComSenhaFraca_DeveRetornarMensagemDeErro()
        {
            //Arrange
            var obj = new RegistrarUsuarioViewModel
            {
                Email = "teste@teste.com",
                Senha = "teste",
                SenhaConfirmacao = "teste"
            };
            string strData = JsonConvert.SerializeObject(obj);
            var contentData = new StringContent(strData, Encoding.UTF8, "application/json");
            
            //Act
            var postResponse = await _testsFixture.Client.PostAsync("/api/identidade/nova-conta", contentData);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);
        }


        [Fact(DisplayName = "Realizar cadastro com sucesso"), TestPriority(1)]
        [Trait("Categoria", "Integração API - Usuário")]
        public async void Usuario_RealizarCadastro_DeveExecutarComSucesso()
        {
            //Arrange
            var obj = new RegistrarUsuarioViewModel
            {
                Email = "teste@teste.com",
                Senha = "Teste@123",
                SenhaConfirmacao = "Teste@123"
            };
            string strData = JsonConvert.SerializeObject(obj);
            var contentData = new StringContent(strData, Encoding.UTF8, "application/json");

            _testsFixture.UsuarioEmail = obj.Email;
            _testsFixture.UsuarioSenha = obj.Senha;

            //Act
            var postResponse = await _testsFixture.Client.PostAsync("/api/identidade/nova-conta", contentData);

            //Assert
            Assert.Equal(HttpStatusCode.OK, postResponse.StatusCode);
        }

        [Fact(DisplayName = "Realizar login com sucesso"), TestPriority(2)]
        [Trait("Categoria", "Integração API - Usuário")]
        public async void Usuario_RealizarLogin_DeveExecutarComSucesso()
        {
            // Arrange
            var obj = new LoginUsuarioViewModel
            {
                Email = _testsFixture.UsuarioEmail,
                Senha = _testsFixture.UsuarioSenha
            };

            string strData = JsonConvert.SerializeObject(obj);
            var contentData = new StringContent(strData, Encoding.UTF8, "application/json");

            // Act
            var postResponse = await _testsFixture.Client.PostAsync("/api/identidade/autenticar", contentData);

            // Assert
            Assert.Equal(HttpStatusCode.OK, postResponse.StatusCode);
        }

        [Fact(DisplayName = "Realizar login com falha, usuário não cadastrado")]
        [Trait("Categoria", "Integração API - Usuário")]
        public async void Usuario_RealizarLogin_DeveFalharPorNaoTerCadastro()
        {
            // Arrange
            var obj = new RegistrarUsuarioViewModel
            {
                Email = "teste2@teste.com",
                Senha = "Teste@123",
                SenhaConfirmacao = "Teste@123"
            };
            string strData = JsonConvert.SerializeObject(obj);
            var contentData = new StringContent(strData, Encoding.UTF8, "application/json");

            // Act
            var postResponse = await _testsFixture.Client.PostAsync("/api/identidade/autenticar", contentData);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);
        }
    }
}
