﻿using EasonEetwViewer.HttpRequest.Dto.ApiResponse.Record.Contract;
using EasonEetwViewer.HttpRequest.Dto.ApiResponse.ResponseBase;

namespace EasonEetwViewer.HttpRequest.Dto.ApiResponse.Response;
/// <summary>
/// Represents the result of an API call on <c>contract.list</c> API.
/// </summary>
public record ContractList : ListBase<Contract>;