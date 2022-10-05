# Unimake.Wkhtmltopdf

Este projeto implementa uma versão mais simplificada da biblioteca https://github.com/fpanaccia/Wkhtmltopdf.NetCore para converter html em pdf, apenas para windows.<br/>
Obrigado [fpanaccia](https://github.com/fpanaccia) por compartilhar a versão completa para .NET<br/>
Utiliza o projeto https://wkhtmltopdf.org/ para a conversão.

# Como utilizar

Instale o pacote https://www.nuget.org/packages/Unimake.Wkhtmltopdf

Os arquivos da biblioteca [Wkhtmltopdf](https://wkhtmltopdf.org/) devem estar disponibilizados no seu projeto na seguinte estrutura
        
        ├── Example.sln
        |   ├── Example.csproj
        |   ├──── Wkhtmltox
        |   |     ├── win-x64 <<-- Este diretório é obrigatório
        |   |     |   ├── wkhtmltopdf.exe
        |   |     |   └── wkhtmltox.dll
Para baixar as versões utilizadas na biblioteca, baixe em:<br/>
[wkhtmltopdf.exe](https://github.com/Unimake/Unimake.Wkhtmltopdf/raw/main/source/Unimake.Wkhtmltopdf/Wkhtmltox/win-x64/wkhtmltopdf.exe) <br/>
[wkhtmltox.dll](https://github.com/Unimake/Unimake.Wkhtmltopdf/raw/main/source/Unimake.Wkhtmltopdf/Wkhtmltox/win-x64/wkhtmltox.dll)

Para baixar versões mais atuais: https://wkhtmltopdf.org/downloads.html

Estes arquivos devem ser incluídos na sua solução e marcados como "Copy Always" para serem copiados para o diretório de build.<br/>
Se publicados em um servidor web, os mesmos deverão ser enviados juntamente com o pacote de distribuição. <br/>
**O diretório "win-x64" é obrigatório**

Esta estrutrura de arquivos, é a estrutrua padrão desta biblioteca, caso queira mudar, passe a configuração através das opções definidas em [ConvertOptions.WkhtmltoxPath](https://github.com/Unimake/Unimake.Wkhtmltopdf/blob/99c60c1ab58b7bd493f09062f05a4b1ebe2acbda/source/Unimake.Wkhtmltopdf/ConvertOptions.cs#L20)

Exemplo de código com biblioteca e diretórios alterados

```csharp
var html = "<p>This is a paragraph.</p>";
var options = new ConvertOptions
{
	WkhtmltoxPath = "libs\\MyLibFolder"
};

var bytes = new HtmlToPdfConverter(options).GetPDFAsByteArray(html);
Assert.NotNull(bytes);
```

Recuperando um array de bytes e salvando em um arquivo .pdf

```csharp
[Fact]
public void GetPDFAsByteArray()
{
	var html = "<p>This is a paragraph.</p>";
	var bytes = new HtmlToPdfConverter().GetPDFAsByteArray(html);
	Assert.NotNull(bytes);

	var fi = new FileInfo("test.pdf");

	if(fi.Exists)
	{
		fi.Delete();
	}

	File.WriteAllBytes(fi.FullName, bytes);

	//Open pdf file
	_ = Process.Start(new ProcessStartInfo
	{
		UseShellExecute = true,
		FileName = fi.FullName
	});
}
```

Recuperando uma string padrão Base64

```csharp
[Fact]
public void GetPDFAsBase64()
{
	var html = "<p>This is a paragraph.</p>";
	var pdf = new HtmlToPdfConverter().GetPDFAsBase64(html);
	Assert.NotNull(pdf);

	//save to file
	var bytes = Convert.FromBase64String(pdf);
	Assert.NotNull(bytes);

	var fi = new FileInfo("test.pdf");

	if(fi.Exists)
	{
		fi.Delete();
	}

	File.WriteAllBytes(fi.FullName, bytes);

	//Open pdf file
	_ = Process.Start(new ProcessStartInfo
	{
		UseShellExecute = true,
		FileName = fi.FullName
	});
}
```

Veja outoros exemplos de uso em [Unimake.Wkhtmltopdf.Test](https://github.com/Unimake/Unimake.Wkhtmltopdf/tree/main/source/Unimake.Wkhtmltopdf.Test)
