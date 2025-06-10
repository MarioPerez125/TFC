import 'package:dio/dio.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class ApiResponse {
  final int statusCode;
  final dynamic data;
  final String? error;

  ApiResponse({required this.statusCode, this.data, this.error});
}

class ApiClient {
  static const String baseUrl = 'http://10.0.2.2:5263/api/';
  final Dio _dio = Dio(BaseOptions(baseUrl: baseUrl));
  final FlutterSecureStorage _secureStorage = const FlutterSecureStorage();

  Future<String?> _getToken() async {
    return await _secureStorage.read(key: 'jwt_token');
  }

  Future<ApiResponse> post(
    String endpoint, {
    required Map<String, dynamic> body,
    bool requiresAuth = true,
  }) async {
    try {
      final headers = <String, dynamic>{'Content-Type': 'application/json'};
      if (requiresAuth) {
        final token = await _getToken();
        if (token != null) {
          headers['Authorization'] = 'Bearer $token';
        }
      }
      final response = await _dio.post(
        endpoint, 
        data: body,
        options: Options(headers: headers),
      );
      print('POST a: ${_dio.options.baseUrl}$endpoint');
      print('Body: $body');
      return ApiResponse(
        statusCode: response.statusCode ?? 0,
        data: response.data,
      );
    } on DioException catch (e) {
      return ApiResponse(
        statusCode: e.response?.statusCode ?? 500,
        error: e.message,
        data: e.response?.data,
      );
    }
  }

  Future<ApiResponse> get(
    String endpoint, {
    Map<String, dynamic>? queryParams,
    bool requiresAuth = false,
  }) async {
    try {
      final headers = <String, dynamic>{};
      if (requiresAuth) {
        final token = await _getToken();
        if (token != null) {
          headers['Authorization'] = 'Bearer $token';
        }
      }
      final cleanEndpoint = endpoint.startsWith('/')
          ? endpoint.substring(1)
          : endpoint;
      print('Llamando a: ${_dio.options.baseUrl}$cleanEndpoint');
      final response = await _dio.get(
        cleanEndpoint,
        queryParameters: queryParams,
        options: Options(headers: headers),
      );
      return ApiResponse(
        statusCode: response.statusCode ?? 0,
        data: response.data,
      );
    } on DioException catch (e) {
      return ApiResponse(
        statusCode: e.response?.statusCode ?? 500,
        error: e.message,
        data: e.response?.data,
      );
    }
  }
}
