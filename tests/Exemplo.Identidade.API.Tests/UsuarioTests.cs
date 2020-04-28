using Exemplo.Identidade.API.ViewModels;
using Exemplo.Identidade.Tests.Config;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace Exemplo.Identidade.API.Tests
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class UsuarioTests
    {
        private readonly IntegrationTestsFixture<StartupApiTests> _testsFixture;
        public UsuarioTests(IntegrationTestsFixture<StartupApiTests> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Realizar cadastro com sucesso")]
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

            //Act
            var postResponse = await _testsFixture.Client.PostAsync("/api/identidade/nova-conta", contentData);

            //Assert
            Assert.Equal(HttpStatusCode.OK, postResponse.StatusCode);
        }
    }
}
