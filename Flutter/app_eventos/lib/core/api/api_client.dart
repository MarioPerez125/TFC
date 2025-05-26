import 'dart:convert';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:http/http.dart' as http;
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

import 'api_response.dart';

class ApiClient {
  static final String? _baseUrl = dotenv.env['API_BASE_URL'];
  final FlutterSecureStorage _secureStorage = const FlutterSecureStorage();
  final http.Client _client;

  ApiClient() : _client = http.Client();

  // Método para obtener el token JWT almacenado
  Future<String?> _getToken() async {
    return await _secureStorage.read(key: 'jwt_token');
  }

  // Interceptor para añadir el token a las peticiones
  Future<http.Response> _interceptedRequest(
    Future<http.Response> Function() requestFn,
  ) async {
    final token = await _getToken();
    final response = await requestFn();

    // Si el token expiró (401), podrías implementar aquí un refresh token
    if (response.statusCode == 401) {
      // Lógica para manejar token expirado
      throw Exception('Sesión expirada. Por favor inicie sesión nuevamente.');
    }

    return response;
  }

  Future<ApiResponse> post(
    String endpoint, {
    required Map<String, dynamic> body,
    bool requiresAuth = true,
  }) async {
    try {
      final url = Uri.parse('$_baseUrl/$endpoint');
      final headers = {'Content-Type': 'application/json'};

      if (requiresAuth) {
        final token = await _getToken();
        if (token != null) {
          headers['Authorization'] = 'Bearer $token';
        }
      }

      final response = await _interceptedRequest(
        () => http.post(
          url,
          headers: headers,
          body: jsonEncode(body),
        ),
      );

      return ApiResponse(
        statusCode: response.statusCode,
        data: jsonDecode(response.body),
      );
    } catch (e) {
      return ApiResponse(
        statusCode: 500,
        error: e.toString(),
      );
    }
  }

  Future<ApiResponse> get(
    String endpoint, {
    Map<String, String>? queryParams,
    bool requiresAuth = true,
  }) async {
    try {
      final uri = Uri.parse('$_baseUrl/$endpoint').replace(
        queryParameters: queryParams,
      );
      final headers = {};

      if (requiresAuth) {
        final token = await _getToken();
        if (token != null) {
          headers['Authorization'] = 'Bearer $token';
        }
      }

      final response = await _interceptedRequest(
        () => http.get(uri, headers: headers as Map<String, String>?),
      );

      return ApiResponse(
        statusCode: response.statusCode,
        data: jsonDecode(response.body),
      );
    } catch (e) {
      return ApiResponse(
        statusCode: 500,
        error: e.toString(),
      );
    }
  }

  // Agrega aquí otros métodos (put, delete, etc.) con la misma estructura
}