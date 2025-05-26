import 'dart:convert';

import 'package:http/http.dart' as http;

class ApiResponse {
  final int statusCode;
  final dynamic data;
  final String? error;

  ApiResponse({
    required this.statusCode,
    this.data,
    this.error,
  });

  factory ApiResponse.fromResponse(http.Response response) {
    try {
      final dynamic responseData = jsonDecode(response.body);
      return ApiResponse(
        statusCode: response.statusCode,
        data: responseData,
        error: response.statusCode >= 400 ? responseData['message'] ?? 'Error desconocido' : null,
      );
    } catch (e) {
      return ApiResponse(
        statusCode: response.statusCode,
        error: 'Error parsing response: ${e.toString()}',
      );
    }
  }
}