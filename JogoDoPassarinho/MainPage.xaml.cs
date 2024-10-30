using System.Threading.Channels;

namespace JogoDoPassarinho;

public partial class MainPage : ContentPage
{

	const int gravidade = 4;
	const int tempoEntreFrames = 25;
	bool estaMorto = true;
	double larguraJanela = 0;
	double alturaJanela = 0;
	int velocidade = 10;
	const int forcaPulo = 30;
	const int maxTempoPulando = 3;
	int tempoPulando = 0;
	bool estaPulando = false;
	int score = 0;
	const int aberturaMinima = 250;



	public MainPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		SoundHelper.Play("fundo.wav", true);
    }

    void AplicaGravidade()
	{
		paimon.TranslationY += gravidade;
	}

	private async void Desenha()
	{
		while (!estaMorto)
		{
			if (estaPulando)
				AplicaPulo();
			else
				AplicaGravidade();
			GerenciaCanos();
			if (VerificaColisaoCano())
			{
				estaMorto = true;
				FrameGameOver.IsVisible = true;
				break;
			}
			await Task.Delay(tempoEntreFrames);
		}
	}

	void Oi(object s, TappedEventArgs e)
	{
		FrameGameOver.IsVisible = false;
		estaMorto = false;
		Inicializar();
		Desenha();
	}

void Inicializar()
	{
		pilarnormal.TranslationX = -larguraJanela;
		pilarvirado.TranslationX = -larguraJanela;
		paimon.TranslationY = 0;
		GerenciaCanos();
	}

protected override void OnSizeAllocated(double w, double h)
	{
		base.OnSizeAllocated(w, h);
		larguraJanela = w;
		alturaJanela = h;
		if (h > 0)
		{
			pilarvirado.HeightRequest = h - graminha.HeightRequest;
			pilarnormal.HeightRequest = h - graminha.HeightRequest;
		}
	}

	void GerenciaCanos()
  {
    pilarnormal.TranslationX -= velocidade;
    pilarvirado.TranslationX -= velocidade;
    if (pilarvirado.TranslationX < -larguraJanela)
    {
      pilarvirado.TranslationX = 0;
      pilarnormal.TranslationX = 0;

      var alturaMaxima = -(pilarnormal.HeightRequest * 0.5);
      var alturaMinima = -(pilarnormal.HeightRequest * 0.8);

      pilarvirado.TranslationY  = Random.Shared.Next((int)alturaMinima, (int)alturaMaxima);
      pilarnormal.TranslationY = pilarvirado.TranslationY + aberturaMinima + pilarnormal.HeightRequest;

      score++;
      labelScore.Text = "Score: " + score.ToString("D3");
      if (score % 4 == 0)
        velocidade++;
    }
  }

  	bool VerificaColisao()
	{
		return VerificaColisaoTeto() ||
				VerificaColisaoChao() ||
				VerificaColisaopilarvirado() ||
				VerificaColisaopilarnormal();
	}

		bool VerificaColisaoTeto()
	{
		var minY = -alturaJanela / 2;
		if (paimon.TranslationY <= minY)
			return true;
		else
			return false;
	}

	bool VerificaColisaoCano()
  {
    if (VerificaColisaopilarnormal() || VerificaColisaopilarvirado())
      return true;
    else
      return false;
  }
	
	bool VerificaColisaoChao()
	{
		var maxY = alturaJanela / 2 - graminha.HeightRequest;
		if (paimon.TranslationY <= maxY)
			return true;
		else
			return false;
	}

	void AplicaPulo()
	{
		paimon.TranslationY -= forcaPulo;
		tempoPulando++;
		if (tempoPulando >= maxTempoPulando)
		{
			estaPulando = false;
			tempoPulando = 0;
		}
	}

	 bool VerificaColisaopilarvirado()
  {
    var posicaoHorizontalpaimon = (larguraJanela / 2) - (paimon.WidthRequest / 2);
    var posicaoVerticalpaimon   = (alturaJanela / 2) - (paimon.HeightRequest / 2) + paimon.TranslationY;

    if (posicaoHorizontalpaimon >= Math.Abs(pilarvirado.TranslationX) - pilarvirado.WidthRequest &&
         posicaoHorizontalpaimon <= Math.Abs(pilarvirado.TranslationX) + pilarvirado.WidthRequest &&
         posicaoVerticalpaimon   <= pilarvirado.HeightRequest + pilarvirado.TranslationY)
      return true;
    else
      return false;
  }

	 bool VerificaColisaopilarnormal()
  {
    var posicaoHorizontalpaimon = (larguraJanela / 2) - (paimon.WidthRequest / 2);
    var posicaoVerticalpaimon   = (alturaJanela / 2) + (paimon.HeightRequest / 2) + paimon.TranslationY;

    var yMaxCano = pilarvirado.HeightRequest + pilarvirado.TranslationY + aberturaMinima;

    if (
         posicaoHorizontalpaimon >= Math.Abs(pilarvirado.TranslationX) - pilarvirado.WidthRequest &&
         posicaoHorizontalpaimon <= Math.Abs(pilarvirado.TranslationX) + pilarvirado.WidthRequest &&
         posicaoVerticalpaimon   >= yMaxCano
       )
      return true;
    else
      return false;
  }


	void OnGridClicked(object s, TappedEventArgs a)
	{
		estaPulando = true;
	}

}
