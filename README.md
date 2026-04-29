VarzeaOnline ⚽
Um sistema web completo para organizar e gerenciar campeonatos de FIFA/EA FC entre amigos, com suporte a múltiplos formatos de torneios, automação de rodadas e tabelas de classificação em tempo real.
Mostrar Imagem
Mostrar Imagem
Mostrar Imagem
Mostrar Imagem
📋 Sobre o Projeto
VarzeaOnline é uma aplicação web desenvolvida em ASP.NET MVC que facilita a criação e gerenciamento de campeonatos de FIFA entre jogadores. Com uma interface intuitiva inspirada no design da EA Sports, permite desde a criação do campeonato até o acompanhamento completo das rodadas e classificação.
✨ Destaques

🎮 50+ Times Reais — Times das principais ligas (Premier League, La Liga, Série A, etc.)
🏆 Múltiplos Formatos — Pontos Corridos, Mata-Mata e Fase de Grupos + Mata-Mata
📊 Classificação Automática — Tabela atualiza automaticamente conforme os resultados
👥 Sistema de Usuários — Login e cadastro para múltiplos jogadores
🎨 Design Dark Theme — Interface moderna inspirada no estilo EA FC

🛠️ Tecnologias
TecnologiaVersãoUsoC#9.0Linguagem principalASP.NET MVC5.0Framework webEntity Framework6.0ORM e acesso a dadosSQL Server Express2019+Banco de dadosBootstrap5.2.3Frontend responsivoJavaScriptES6+Interatividade
📦 Requisitos

Visual Studio 2022 ou superior
.NET Framework 4.8 SDK
SQL Server Express 2019 ou posterior
Node.js 14+ (para dependências npm opcionais)

🚀 Como Instalar e Rodar
1. Clonar o Repositório
bashgit clone https://github.com/seu-usuario/varzeaonline.git
cd varzeaonline
2. Restaurar Banco de Dados
Abra o Visual Studio 2022 e navegue até:

Ferramentas → Gerenciador de Pacotes NuGet → Console do Gerenciador de Pacotes

Execute:
powershellUpdate-Database
3. Executar a Aplicação
Pressione F5 ou clique em Iniciar Depuração para rodar o projeto.
A aplicação abrirá em http://localhost:XXXX automaticamente.
4. Criar uma Conta
Na tela inicial, clique em "Criar Conta" e registre-se com email e senha.

Nota: Há um usuário admin pré-cadastrado para testes:

Email: admin@fifa.com
Senha: 123


📖 Como Usar
Criar um Campeonato

Faça login
Na página inicial, clique em "Novo Campeonato"
Preencha:

Nome do campeonato
Formato (Pontos Corridos, Mata-Mata, etc.)
Quantidade de jogadores


Clique em "Criar"

Adicionar Jogadores

Na tela do campeonato, clique em "Jogadores"
Selecione seu nome, escolha um time e clique em "Adicionar"
Repita para todos os jogadores
Quando atingir o limite, clique em "Iniciar Campeonato"

Acompanhar Rodadas

Clique em "Detalhes" do campeonato
Na aba de rodadas, registre os placares
Após registrar todos os placares, clique em "Finalizar Rodada"
A classificação atualiza automaticamente

Ver Classificação
Na página do campeonato, clique em "Classificação" para ver a tabela completa com:

Posição
Jogador e time
Jogos, vitórias, empates, derrotas
Gols pró, contra e saldo
Pontos

🏗️ Estrutura do Projeto
VarzeaOnline/
├── Controllers/
│   ├── HomeController.cs
│   ├── CampeonatoController.cs
│   └── UsuarioController.cs
├── Models/
│   ├── Campeonato.cs
│   ├── Jogador.cs
│   ├── Partida.cs
│   ├── Classificacao.cs
│   ├── Time.cs
│   └── Usuario.cs
├── Views/
│   ├── Home/
│   ├── Campeonato/
│   ├── Usuario/
│   └── Shared/
├── Data/
│   └── ApplicationDbContext.cs
├── Content/
│   └── Site.css (tema EA FC)
└── Web.config
🎨 Design
O projeto utiliza um tema escuro inspirado na EA Sports com:

Paleta de cores: Verde neon (#00e676), Ouro (#ffca28), Azul (#42a5f5)
Fontes: Rajdhani (títulos) e Inter (corpo)
Componentes customizados: ea-card, ea-btn, ea-badge, etc.

📝 Funcionalidades Implementadas

✅ CRUD completo de campeonatos
✅ Sistema de autenticação e autorização
✅ Geração automática de rodadas (round-robin)
✅ Registro manual de resultados
✅ Cálculo automático de classificação
✅ Suporte a múltiplos formatos de torneio
✅ Interface responsiva
✅ Try-catch em operações críticas

🚧 Funcionalidades Futuras

🔲 Estatísticas por jogador (melhor atacante, goleiro, etc.)
🔲 Sistema de notificações
🔲 Integração com API de times reais
🔲 Dashboard de histórico de campeonatos
🔲 Ranking global entre usuários
🔲 Mobile app (React Native)

🐛 Problemas Conhecidos
Nenhum relatado no momento. Se encontrar algum problema, abra uma issue.
🤝 Contribuindo
Contribuições são bem-vindas! Para contribuir:

Faça um fork do projeto
Crie uma branch para sua feature (git checkout -b feature/AmazingFeature)
Commit suas mudanças (git commit -m 'Add some AmazingFeature')
Push para a branch (git push origin feature/AmazingFeature)
Abra um Pull Request

📄 Licença
Este projeto está licenciado sob a MIT License - veja o arquivo LICENSE para detalhes.
👨‍💻 Autor
Vinicius - Desenvolvedor

🙋 Suporte
Tem uma dúvida ou sugestão? Abra uma issue no repositório ou entre em contato!

<p align="center">
  Desenvolvido com ❤️ e muito café ☕
</p>
<p align="center">
  <img src="https://img.shields.io/badge/Made%20with-C%23-blue?style=for-the-badge" alt="C#" />
  <img src="https://img.shields.io/badge/Love-%E2%9D%A4-red?style=for-the-badge" alt="Love" />
</p>
