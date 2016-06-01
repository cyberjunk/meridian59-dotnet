<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<title>BGF Image Service</title>
</head>
<body>
    <h1>BGF Image Service</h1>

    <hr />
    <h2>BGF Frame</h2>
    <h3>Query-URL: /frame/{format}/{file}/{group}/{palette}/{angle}</h3>
    <h3>Examples PNG</h3>
    <table>
    <tbody>
        <tr>
            <td>Name: avsham<br />Format: png<br />Group: 1<br />Palette: 0<br />Angle: 0</td>
            <td>Name: avsham<br />Format: png<br />Group: 1<br />Palette: 0<br />Angle: 512</td>
            <td>Name: avsham<br />Format: png<br />Group: 1<br />Palette: 0<br />Angle: 1024</td>
            <td>Name: avsham<br />Format: png<br />Group: 1<br />Palette: 0<br />Angle: 2048</td>
            <td>Name: avsham<br />Format: png<br />Group: 1<br />Palette: 0<br />Angle: 3072</td>
            <td>Name: avsham<br />Format: png<br />Group: 1<br />Palette: 0<br />Angle: 3584</td>
        </tr>
        <tr>
            <td><img src="frame/png/avsham/1/0/0" /></td>
            <td><img src="frame/png/avsham/1/0/512" /></td>
            <td><img src="frame/png/avsham/1/0/1024" /></td>
            <td><img src="frame/png/avsham/1/0/2048" /></td>
            <td><img src="frame/png/avsham/1/0/3072" /></td>
            <td><img src="frame/png/avsham/1/0/3584" /></td>
        </tr>
    </tbody>
    </table>
    <h3>Examples BMP</h3>
    <table>
    <tbody>
        <tr>
            <td>Name: avsham<br />Format: bmp<br />Group: 11<br />Palette: 78<br />Angle: 0</td>
            <td>Name: avsham<br />Format: bmp<br />Group: 11<br />Palette: 78<br />Angle: 512</td>
            <td>Name: avsham<br />Format: bmp<br />Group: 11<br />Palette: 78<br />Angle: 1024</td>
            <td>Name: avsham<br />Format: bmp<br />Group: 11<br />Palette: 78<br />Angle: 2048</td>
            <td>Name: avsham<br />Format: bmp<br />Group: 11<br />Palette: 78<br />Angle: 3072</td>
            <td>Name: avsham<br />Format: bmp<br />Group: 11<br />Palette: 78<br />Angle: 3584</td>
        </tr>
        <tr>
            <td><img src="frame/bmp/avsham/11/78/0" /></td>
            <td><img src="frame/bmp/avsham/11/78/512" /></td>
            <td><img src="frame/bmp/avsham/11/78/1024" /></td>
            <td><img src="frame/bmp/avsham/11/78/2048" /></td>
            <td><img src="frame/bmp/avsham/11/78/3072" /></td>
            <td><img src="frame/bmp/avsham/11/78/3584" /></td>
        </tr>
    </tbody>
    </table>

    <hr />
    <h2>Object Frame</h2>
    <h3>Query-URL: /object/{file}/{group}/{palette}/{angle}/?subov={file};{group};{palette};{hotspot}&subov={file};{group};{palette};{hotspot}&...</h3>
    <h3>Examples</h3>
    <table>
    <tbody>
        <tr>
            <td>Format: png<br />Group: 11<br />Palette: 78<br />Angle: 0</td>
            <td>Format: png<br />Group: 11<br />Palette: 78<br />Angle: 512</td>
            <td>Format: png<br />Group: 11<br />Palette: 78<br />Angle: 1024</td>
            <td>Format: png<br />Group: 11<br />Palette: 78<br />Angle: 2048</td>
            <td>Format: png<br />Group: 11<br />Palette: 78<br />Angle: 3072</td>
            <td>Format: png<br />Group: 11<br />Palette: 78<br />Angle: 3584</td>
        </tr>
        <tr>
            <td><img src="object/bta/1/236/0/?subov=bla;1;236;31&subov=bra;17;236;21&subov=bfe;1;236;21&subov=phax;1;3;1&subov=pmax;1;3;12&subov=pecx;1;3;11&subov=pnax;1;3;14&subov=ptad;1;10;13" /></td>
            <td><img src="object/bta/1/236/512/?subov=bla;1;236;31&subov=bra;17;236;21&subov=bfe;1;236;21&subov=phax;1;3;1&subov=pmax;1;3;12&subov=pecx;1;3;11&subov=pnax;1;3;14&subov=ptad;1;10;13" /></td>           
            <td><img src="object/bta/1/236/1024/?subov=bla;1;236;31&subov=bra;17;236;21&subov=bfe;1;236;21&subov=phax;1;3;1&subov=pmax;1;3;12&subov=pecx;1;3;11&subov=pnax;1;3;14&subov=ptad;1;10;13" /></td>
            <td><img src="object/bta/1/236/2048/?subov=bla;1;236;31&subov=bra;17;236;21&subov=bfe;1;236;21&subov=phax;1;3;1&subov=pmax;1;3;12&subov=pecx;1;3;11&subov=pnax;1;3;14&subov=ptad;1;10;13" /></td>
            <td><img src="object/bta/1/236/3072/?subov=bla;1;236;31&subov=bra;17;236;21&subov=bfe;1;236;21&subov=phax;1;3;1&subov=pmax;1;3;12&subov=pecx;1;3;11&subov=pnax;1;3;14&subov=ptad;1;10;13" /></td>
            <td><img src="object/bta/1/236/3584/?subov=bla;1;236;31&subov=bra;17;236;21&subov=bfe;1;236;21&subov=phax;1;3;1&subov=pmax;1;3;12&subov=pecx;1;3;11&subov=pnax;1;3;14&subov=ptad;1;10;13" /></td>            
        </tr>
    </tbody>
    </table>
</body>
</html>
