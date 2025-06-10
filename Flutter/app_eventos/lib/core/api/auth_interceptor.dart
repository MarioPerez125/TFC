import 'package:http/http.dart' as http;

class AuthInterceptor extends http.BaseClient {
  final http.Client _innerClient;
  final Future<String?> Function() _getToken;

  AuthInterceptor(this._innerClient, this._getToken);

  @override
  Future<http.StreamedResponse> send(http.BaseRequest request) async {
    final token = await _getToken();
    
    if (token != null) {
      request.headers['Authorization'] = 'Bearer $token';
    }
    
    return _innerClient.send(request);
  }
}