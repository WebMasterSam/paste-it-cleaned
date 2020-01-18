// tslint:disable

import * as request from "superagent";
import {
    SuperAgentStatic,
    SuperAgentRequest,
    Response
} from "superagent";

export type RequestHeaders = {
    [header: string]: string;
}
export type RequestHeadersHandler = (headers: RequestHeaders) => RequestHeaders;

export type ConfigureAgentHandler = (agent: SuperAgentStatic) => SuperAgentStatic;

export type ConfigureRequestHandler = (agent: SuperAgentRequest) => SuperAgentRequest;

export type CallbackHandler = (err: any, res ? : request.Response) => void;

export type AccountRequest = {
    'any' ? : string;
} & {
    [key: string]: any;
};

export type PagedListHitEntity = {
    'results' ? : Array < HitEntity >
    ;
    'currentPage' ? : number;
    'pageCount' ? : number;
    'pageSize' ? : number;
    'rowCount' ? : number;
    'firstRowOnPage' ? : number;
    'lastRowOnPage' ? : number;
} & {
    [key: string]: any;
};

export type HitEntity = {
    'hitId' ? : string;
    'date' ? : string;
    'ip' ? : string;
    'price' ? : number;
    'type' ? : string;
    'referer' ? : string;
} & {
    [key: string]: any;
};

export type HitDailyEntity = {
    'hitDailyId' ? : string;
    'date' ? : string;
    'countExcel' ? : number;
    'countWord' ? : number;
    'countPowerPoint' ? : number;
    'countWeb' ? : number;
    'countText' ? : number;
    'countImage' ? : number;
    'countOther' ? : number;
    'totalPrice' ? : number;
} & {
    [key: string]: any;
};

export type InvoiceEntity = {
    'invoiceId' ? : string;
    'number' ? : number;
    'date' ? : string;
    'price' ? : number;
    'taxes' ? : number;
    'total' ? : number;
    'paid' ? : boolean;
    'paidOn' ? : string;
    'payment' ? : PaymentEntity;
} & {
    [key: string]: any;
};

export type PaymentEntity = {
    'paymentId' ? : string;
    'trxNumber' ? : string;
    'total' ? : number;
    'paymentMethod' ? : PaymentMethodEntity;
} & {
    [key: string]: any;
};

export type PaymentMethodEntity = {
    'paymentMethodId' ? : string;
    'type' ? : string;
    'paymentFailed' ? : boolean;
} & {
    [key: string]: any;
};

export type BillingRequest = {
    'any' ? : string;
} & {
    [key: string]: any;
};

export type ApiKeyEntity = {
    'apiKeyId' ? : string;
    'key' ? : string;
    'expiresOn' ? : string;
    'domains' ? : Array < DomainEntity >
    ;
} & {
    [key: string]: any;
};

export type DomainEntity = {
    'domainId' ? : string;
    'name' ? : string;
} & {
    [key: string]: any;
};

export type PluginApiKeyRequest = {
    'apiKey' ? : ApiKeyEntity;
} & {
    [key: string]: any;
};

export type ActionResult = {} & {
    [key: string]: any;
};

export type PluginApiKeyDomainRequest = {
    'domainName' ? : string;
} & {
    [key: string]: any;
};

export type ConfigEntity = {
    'configId' ? : string;
    'name' ? : string;
    'embedExternalImages' ? : boolean;
    'removeEmptyTags' ? : boolean;
    'removeSpanTags' ? : boolean;
    'removeClassNames' ? : boolean;
    'removeIframes' ? : boolean;
    'removeTagAttributes' ? : boolean;
} & {
    [key: string]: any;
};

export type PluginConfigRequest = {
    'config' ? : ConfigEntity;
} & {
    [key: string]: any;
};

export type TimeZoneEntity = {
    'name' ? : string;
    'displayName' ? : string;
    'offset' ? : string;
} & {
    [key: string]: any;
};

export type Response_GetAnalyticsHitsDaily_200 = Array < HitDailyEntity >
;

export type Response_GetInvoices_200 = Array < InvoiceEntity >
;

export type Response_GetDashboardHitsDaily_200 = Array < HitDailyEntity >
;

export type Response_GetDashboardInvoices_200 = Array < InvoiceEntity >
;

export type Response_GetApiKeys_200 = Array < ApiKeyEntity >
;

export type Response_GetTimeZones_200 = Array < TimeZoneEntity >
;

export type Response_GetTimeZonesByCountry_200 = Array < TimeZoneEntity >
;

export type Logger = {
    log: (line: string) => any
};

export interface ResponseWithBody < S extends number, T > extends Response {
    status: S;
    body: T;
}

export type QueryParameters = {
    [param: string]: any
};

export interface CommonRequestOptions {
    $queryParameters ? : QueryParameters;
    $domain ? : string;
    $path ? : string | ((path: string) => string);
    $retries ? : number; // number of retries; see: https://github.com/visionmedia/superagent/blob/master/docs/index.md#retrying-requests
    $timeout ? : number; // request timeout in milliseconds; see: https://github.com/visionmedia/superagent/blob/master/docs/index.md#timeouts
    $deadline ? : number; // request deadline in milliseconds; see: https://github.com/visionmedia/superagent/blob/master/docs/index.md#timeouts
}

/**
 * 
 * @class BackendEntity
 * @param {(string)} [domainOrOptions] - The project domain.
 */
export class BackendEntity {

    private domain: string = "";
    private errorHandlers: CallbackHandler[] = [];
    private requestHeadersHandler ? : RequestHeadersHandler;
    private configureAgentHandler ? : ConfigureAgentHandler;
    private configureRequestHandler ? : ConfigureRequestHandler;

    constructor(domain ? : string, private logger ? : Logger) {
        if (domain) {
            this.domain = domain;
        }
    }

    getDomain() {
        return this.domain;
    }

    addErrorHandler(handler: CallbackHandler) {
        this.errorHandlers.push(handler);
    }

    setRequestHeadersHandler(handler: RequestHeadersHandler) {
        this.requestHeadersHandler = handler;
    }

    setConfigureAgentHandler(handler: ConfigureAgentHandler) {
        this.configureAgentHandler = handler;
    }

    setConfigureRequestHandler(handler: ConfigureRequestHandler) {
        this.configureRequestHandler = handler;
    }

    private request(method: string, url: string, body: any, headers: RequestHeaders, queryParameters: QueryParameters, form: any, reject: CallbackHandler, resolve: CallbackHandler, opts: CommonRequestOptions) {
        if (this.logger) {
            this.logger.log(`Call ${method} ${url}`);
        }

        const agent = this.configureAgentHandler ?
            this.configureAgentHandler(request.default) :
            request.default;

        let req = agent(method, url);
        if (this.configureRequestHandler) {
            req = this.configureRequestHandler(req);
        }

        req = req.query(queryParameters);

        if (body) {
            req.send(body);

            if (typeof(body) === 'object' && !(body.constructor.name === 'Buffer')) {
                headers['Content-Type'] = 'application/json';
            }
        }

        if (Object.keys(form).length > 0) {
            req.type('form');
            req.send(form);
        }

        if (this.requestHeadersHandler) {
            headers = this.requestHeadersHandler({
                ...headers
            });
        }

        req.set(headers);

        if (opts.$retries && opts.$retries > 0) {
            req.retry(opts.$retries);
        }

        if (opts.$timeout && opts.$timeout > 0 || opts.$deadline && opts.$deadline > 0) {
            req.timeout({
                deadline: opts.$deadline,
                response: opts.$timeout
            });
        }

        req.end((error, response) => {
            // an error will also be emitted for a 4xx and 5xx status code
            // the error object will then have error.status and error.response fields
            // see superagent error handling: https://github.com/visionmedia/superagent/blob/master/docs/index.md#error-handling
            if (error) {
                reject(error);
                this.errorHandlers.forEach(handler => handler(error));
            } else {
                resolve(response);
            }
        });
    }

    private convertParameterCollectionFormat < T > (param: T, collectionFormat: string | undefined): T | string {
        if (Array.isArray(param) && param.length >= 2) {
            switch (collectionFormat) {
                case "csv":
                    return param.join(",");
                case "ssv":
                    return param.join(" ");
                case "tsv":
                    return param.join("\t");
                case "pipes":
                    return param.join("|");
                default:
                    return param;
            }
        }

        return param;
    }

    GetAccountURL(parameters: {
        'authorization' ? : string,
    } & CommonRequestOptions): string {
        let queryParameters: QueryParameters = {};
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/account';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        if (parameters.$queryParameters) {
            queryParameters = {
                ...queryParameters,
                ...parameters.$queryParameters
            };
        }

        let keys = Object.keys(queryParameters);
        return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '');
    }

    /**
     * 
     * @method
     * @name BackendEntity#GetAccount
     * @param {string} authorization - 
     */
    GetAccount(parameters: {
        'authorization' ? : string,
    } & CommonRequestOptions): Promise < ResponseWithBody < 200, void >> {
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/account';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        let body: any;
        let queryParameters: QueryParameters = {};
        let headers: RequestHeaders = {};
        let form: any = {};
        return new Promise((resolve, reject) => {
            headers['Accept'] = '';
            headers['Content-Type'] = '';

            if (parameters['authorization'] !== undefined) {
                headers['authorization'] = parameters['authorization'];
            }

            if (parameters.$queryParameters) {
                queryParameters = {
                    ...queryParameters,
                    ...parameters.$queryParameters
                };
            }

            this.request('GET', domain + path, body, headers, queryParameters, form, reject, resolve, parameters);
        });
    }

    PostAccountURL(parameters: {
        'authorization' ? : string,
        'obj' ? : AccountRequest,
    } & CommonRequestOptions): string {
        let queryParameters: QueryParameters = {};
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/account';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        if (parameters.$queryParameters) {
            queryParameters = {
                ...queryParameters,
                ...parameters.$queryParameters
            };
        }

        queryParameters = {};

        let keys = Object.keys(queryParameters);
        return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '');
    }

    /**
     * 
     * @method
     * @name BackendEntity#PostAccount
     * @param {string} authorization - 
     * @param {} obj - 
     */
    PostAccount(parameters: {
        'authorization' ? : string,
        'obj' ? : AccountRequest,
    } & CommonRequestOptions): Promise < ResponseWithBody < 200, void >> {
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/account';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        let body: any;
        let queryParameters: QueryParameters = {};
        let headers: RequestHeaders = {};
        let form: any = {};
        return new Promise((resolve, reject) => {
            headers['Accept'] = '';
            headers['Content-Type'] = 'application/json-patch+json';

            if (parameters['authorization'] !== undefined) {
                headers['authorization'] = parameters['authorization'];
            }

            if (parameters['obj'] !== undefined) {
                body = parameters['obj'];
            }

            if (parameters.$queryParameters) {
                queryParameters = {
                    ...queryParameters,
                    ...parameters.$queryParameters
                };
            }

            form = queryParameters;
            queryParameters = {};

            this.request('POST', domain + path, body, headers, queryParameters, form, reject, resolve, parameters);
        });
    }

    GetUserURL(parameters: {
        'authorization' ? : string,
    } & CommonRequestOptions): string {
        let queryParameters: QueryParameters = {};
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/account/user';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        if (parameters.$queryParameters) {
            queryParameters = {
                ...queryParameters,
                ...parameters.$queryParameters
            };
        }

        let keys = Object.keys(queryParameters);
        return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '');
    }

    /**
     * 
     * @method
     * @name BackendEntity#GetUser
     * @param {string} authorization - 
     */
    GetUser(parameters: {
        'authorization' ? : string,
    } & CommonRequestOptions): Promise < ResponseWithBody < 200, void >> {
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/account/user';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        let body: any;
        let queryParameters: QueryParameters = {};
        let headers: RequestHeaders = {};
        let form: any = {};
        return new Promise((resolve, reject) => {
            headers['Accept'] = '';
            headers['Content-Type'] = '';

            if (parameters['authorization'] !== undefined) {
                headers['authorization'] = parameters['authorization'];
            }

            if (parameters.$queryParameters) {
                queryParameters = {
                    ...queryParameters,
                    ...parameters.$queryParameters
                };
            }

            this.request('GET', domain + path, body, headers, queryParameters, form, reject, resolve, parameters);
        });
    }

    GetAnalyticsHitsURL(parameters: {
        'authorization' ? : string,
        'page' ? : number,
        'pageSize' ? : number,
    } & CommonRequestOptions): string {
        let queryParameters: QueryParameters = {};
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/analytics/hits';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        if (parameters['page'] !== undefined) {
            queryParameters['page'] = this.convertParameterCollectionFormat(
                parameters['page'],
                ''
            );
        }

        if (parameters['pageSize'] !== undefined) {
            queryParameters['pageSize'] = this.convertParameterCollectionFormat(
                parameters['pageSize'],
                ''
            );
        }

        if (parameters.$queryParameters) {
            queryParameters = {
                ...queryParameters,
                ...parameters.$queryParameters
            };
        }

        let keys = Object.keys(queryParameters);
        return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '');
    }

    /**
     * 
     * @method
     * @name BackendEntity#GetAnalyticsHits
     * @param {string} authorization - 
     * @param {integer} page - 
     * @param {integer} pageSize - 
     */
    GetAnalyticsHits(parameters: {
        'authorization' ? : string,
        'page' ? : number,
        'pageSize' ? : number,
    } & CommonRequestOptions): Promise < ResponseWithBody < 200, PagedListHitEntity > | ResponseWithBody < 401, void > | ResponseWithBody < 403, void > | ResponseWithBody < 500, void >> {
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/analytics/hits';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        let body: any;
        let queryParameters: QueryParameters = {};
        let headers: RequestHeaders = {};
        let form: any = {};
        return new Promise((resolve, reject) => {
            headers['Accept'] = 'text/plain, application/json, text/json';
            headers['Content-Type'] = '';

            if (parameters['authorization'] !== undefined) {
                headers['authorization'] = parameters['authorization'];
            }

            if (parameters['page'] !== undefined) {
                queryParameters['page'] = this.convertParameterCollectionFormat(
                    parameters['page'],
                    ''
                );
            }

            if (parameters['pageSize'] !== undefined) {
                queryParameters['pageSize'] = this.convertParameterCollectionFormat(
                    parameters['pageSize'],
                    ''
                );
            }

            if (parameters.$queryParameters) {
                queryParameters = {
                    ...queryParameters,
                    ...parameters.$queryParameters
                };
            }

            this.request('GET', domain + path, body, headers, queryParameters, form, reject, resolve, parameters);
        });
    }

    GetAnalyticsHitsDailyURL(parameters: {
        'authorization' ? : string,
        'startDate' ? : string,
        'endDate' ? : string,
    } & CommonRequestOptions): string {
        let queryParameters: QueryParameters = {};
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/analytics/hits/daily';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        if (parameters['startDate'] !== undefined) {
            queryParameters['startDate'] = this.convertParameterCollectionFormat(
                parameters['startDate'],
                ''
            );
        }

        if (parameters['endDate'] !== undefined) {
            queryParameters['endDate'] = this.convertParameterCollectionFormat(
                parameters['endDate'],
                ''
            );
        }

        if (parameters.$queryParameters) {
            queryParameters = {
                ...queryParameters,
                ...parameters.$queryParameters
            };
        }

        let keys = Object.keys(queryParameters);
        return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '');
    }

    /**
     * 
     * @method
     * @name BackendEntity#GetAnalyticsHitsDaily
     * @param {string} authorization - 
     * @param {string} startDate - 
     * @param {string} endDate - 
     */
    GetAnalyticsHitsDaily(parameters: {
        'authorization' ? : string,
        'startDate' ? : string,
        'endDate' ? : string,
    } & CommonRequestOptions): Promise < ResponseWithBody < 200, Response_GetAnalyticsHitsDaily_200 > | ResponseWithBody < 401, void > | ResponseWithBody < 403, void > | ResponseWithBody < 500, void >> {
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/analytics/hits/daily';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        let body: any;
        let queryParameters: QueryParameters = {};
        let headers: RequestHeaders = {};
        let form: any = {};
        return new Promise((resolve, reject) => {
            headers['Accept'] = 'text/plain, application/json, text/json';
            headers['Content-Type'] = '';

            if (parameters['authorization'] !== undefined) {
                headers['authorization'] = parameters['authorization'];
            }

            if (parameters['startDate'] !== undefined) {
                queryParameters['startDate'] = this.convertParameterCollectionFormat(
                    parameters['startDate'],
                    ''
                );
            }

            if (parameters['endDate'] !== undefined) {
                queryParameters['endDate'] = this.convertParameterCollectionFormat(
                    parameters['endDate'],
                    ''
                );
            }

            if (parameters.$queryParameters) {
                queryParameters = {
                    ...queryParameters,
                    ...parameters.$queryParameters
                };
            }

            this.request('GET', domain + path, body, headers, queryParameters, form, reject, resolve, parameters);
        });
    }

    GetInvoiceURL(parameters: {
        'authorization' ? : string,
        'id': string,
    } & CommonRequestOptions): string {
        let queryParameters: QueryParameters = {};
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/billing/invoices/{id}';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        path = path.replace(
            '{id}',
            `${encodeURIComponent(this.convertParameterCollectionFormat(
                        parameters['id'],
                        ''
                    ).toString())}`
        );

        if (parameters.$queryParameters) {
            queryParameters = {
                ...queryParameters,
                ...parameters.$queryParameters
            };
        }

        let keys = Object.keys(queryParameters);
        return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '');
    }

    /**
     * 
     * @method
     * @name BackendEntity#GetInvoice
     * @param {string} authorization - 
     * @param {string} id - 
     */
    GetInvoice(parameters: {
        'authorization' ? : string,
        'id': string,
    } & CommonRequestOptions): Promise < ResponseWithBody < 200, InvoiceEntity > | ResponseWithBody < 401, void > | ResponseWithBody < 403, void > | ResponseWithBody < 500, void >> {
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/billing/invoices/{id}';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        let body: any;
        let queryParameters: QueryParameters = {};
        let headers: RequestHeaders = {};
        let form: any = {};
        return new Promise((resolve, reject) => {
            headers['Accept'] = 'text/plain, application/json, text/json';
            headers['Content-Type'] = '';

            if (parameters['authorization'] !== undefined) {
                headers['authorization'] = parameters['authorization'];
            }

            path = path.replace(
                '{id}',
                `${encodeURIComponent(this.convertParameterCollectionFormat(
                    parameters['id'],
                    ''
                ).toString())}`
            );

            if (parameters['id'] === undefined) {
                reject(new Error('Missing required  parameter: id'));
                return;
            }

            if (parameters.$queryParameters) {
                queryParameters = {
                    ...queryParameters,
                    ...parameters.$queryParameters
                };
            }

            this.request('GET', domain + path, body, headers, queryParameters, form, reject, resolve, parameters);
        });
    }

    GetInvoicesURL(parameters: {
        'authorization' ? : string,
    } & CommonRequestOptions): string {
        let queryParameters: QueryParameters = {};
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/billing/invoices';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        if (parameters.$queryParameters) {
            queryParameters = {
                ...queryParameters,
                ...parameters.$queryParameters
            };
        }

        let keys = Object.keys(queryParameters);
        return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '');
    }

    /**
     * 
     * @method
     * @name BackendEntity#GetInvoices
     * @param {string} authorization - 
     */
    GetInvoices(parameters: {
        'authorization' ? : string,
    } & CommonRequestOptions): Promise < ResponseWithBody < 200, Response_GetInvoices_200 > | ResponseWithBody < 401, void > | ResponseWithBody < 403, void > | ResponseWithBody < 500, void >> {
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/billing/invoices';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        let body: any;
        let queryParameters: QueryParameters = {};
        let headers: RequestHeaders = {};
        let form: any = {};
        return new Promise((resolve, reject) => {
            headers['Accept'] = 'text/plain, application/json, text/json';
            headers['Content-Type'] = '';

            if (parameters['authorization'] !== undefined) {
                headers['authorization'] = parameters['authorization'];
            }

            if (parameters.$queryParameters) {
                queryParameters = {
                    ...queryParameters,
                    ...parameters.$queryParameters
                };
            }

            this.request('GET', domain + path, body, headers, queryParameters, form, reject, resolve, parameters);
        });
    }

    GetPaymentMethodURL(parameters: {
        'authorization' ? : string,
    } & CommonRequestOptions): string {
        let queryParameters: QueryParameters = {};
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/billing/payment-method';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        if (parameters.$queryParameters) {
            queryParameters = {
                ...queryParameters,
                ...parameters.$queryParameters
            };
        }

        let keys = Object.keys(queryParameters);
        return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '');
    }

    /**
     * 
     * @method
     * @name BackendEntity#GetPaymentMethod
     * @param {string} authorization - 
     */
    GetPaymentMethod(parameters: {
        'authorization' ? : string,
    } & CommonRequestOptions): Promise < ResponseWithBody < 200, PaymentMethodEntity > | ResponseWithBody < 401, void > | ResponseWithBody < 403, void > | ResponseWithBody < 500, void >> {
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/billing/payment-method';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        let body: any;
        let queryParameters: QueryParameters = {};
        let headers: RequestHeaders = {};
        let form: any = {};
        return new Promise((resolve, reject) => {
            headers['Accept'] = 'text/plain, application/json, text/json';
            headers['Content-Type'] = '';

            if (parameters['authorization'] !== undefined) {
                headers['authorization'] = parameters['authorization'];
            }

            if (parameters.$queryParameters) {
                queryParameters = {
                    ...queryParameters,
                    ...parameters.$queryParameters
                };
            }

            this.request('GET', domain + path, body, headers, queryParameters, form, reject, resolve, parameters);
        });
    }

    PostPaymentMethodURL(parameters: {
        'authorization' ? : string,
        'obj' ? : BillingRequest,
    } & CommonRequestOptions): string {
        let queryParameters: QueryParameters = {};
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/billing/payment-method';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        if (parameters.$queryParameters) {
            queryParameters = {
                ...queryParameters,
                ...parameters.$queryParameters
            };
        }

        queryParameters = {};

        let keys = Object.keys(queryParameters);
        return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '');
    }

    /**
     * 
     * @method
     * @name BackendEntity#PostPaymentMethod
     * @param {string} authorization - 
     * @param {} obj - 
     */
    PostPaymentMethod(parameters: {
        'authorization' ? : string,
        'obj' ? : BillingRequest,
    } & CommonRequestOptions): Promise < ResponseWithBody < 200, PaymentMethodEntity > | ResponseWithBody < 401, void > | ResponseWithBody < 403, void > | ResponseWithBody < 500, void >> {
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/billing/payment-method';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        let body: any;
        let queryParameters: QueryParameters = {};
        let headers: RequestHeaders = {};
        let form: any = {};
        return new Promise((resolve, reject) => {
            headers['Accept'] = 'text/plain, application/json, text/json';
            headers['Content-Type'] = 'application/json-patch+json';

            if (parameters['authorization'] !== undefined) {
                headers['authorization'] = parameters['authorization'];
            }

            if (parameters['obj'] !== undefined) {
                body = parameters['obj'];
            }

            if (parameters.$queryParameters) {
                queryParameters = {
                    ...queryParameters,
                    ...parameters.$queryParameters
                };
            }

            form = queryParameters;
            queryParameters = {};

            this.request('POST', domain + path, body, headers, queryParameters, form, reject, resolve, parameters);
        });
    }

    GetDashboardHitsURL(parameters: {
        'authorization' ? : string,
    } & CommonRequestOptions): string {
        let queryParameters: QueryParameters = {};
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/dashboard/hits';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        if (parameters.$queryParameters) {
            queryParameters = {
                ...queryParameters,
                ...parameters.$queryParameters
            };
        }

        let keys = Object.keys(queryParameters);
        return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '');
    }

    /**
     * 
     * @method
     * @name BackendEntity#GetDashboardHits
     * @param {string} authorization - 
     */
    GetDashboardHits(parameters: {
        'authorization' ? : string,
    } & CommonRequestOptions): Promise < ResponseWithBody < 200, PagedListHitEntity > | ResponseWithBody < 401, void > | ResponseWithBody < 403, void > | ResponseWithBody < 500, void >> {
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/dashboard/hits';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        let body: any;
        let queryParameters: QueryParameters = {};
        let headers: RequestHeaders = {};
        let form: any = {};
        return new Promise((resolve, reject) => {
            headers['Accept'] = 'text/plain, application/json, text/json';
            headers['Content-Type'] = '';

            if (parameters['authorization'] !== undefined) {
                headers['authorization'] = parameters['authorization'];
            }

            if (parameters.$queryParameters) {
                queryParameters = {
                    ...queryParameters,
                    ...parameters.$queryParameters
                };
            }

            this.request('GET', domain + path, body, headers, queryParameters, form, reject, resolve, parameters);
        });
    }

    GetDashboardHitsDailyURL(parameters: {
        'authorization' ? : string,
        'startDate' ? : string,
        'endDate' ? : string,
    } & CommonRequestOptions): string {
        let queryParameters: QueryParameters = {};
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/dashboard/hits/daily';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        if (parameters['startDate'] !== undefined) {
            queryParameters['startDate'] = this.convertParameterCollectionFormat(
                parameters['startDate'],
                ''
            );
        }

        if (parameters['endDate'] !== undefined) {
            queryParameters['endDate'] = this.convertParameterCollectionFormat(
                parameters['endDate'],
                ''
            );
        }

        if (parameters.$queryParameters) {
            queryParameters = {
                ...queryParameters,
                ...parameters.$queryParameters
            };
        }

        let keys = Object.keys(queryParameters);
        return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '');
    }

    /**
     * 
     * @method
     * @name BackendEntity#GetDashboardHitsDaily
     * @param {string} authorization - 
     * @param {string} startDate - 
     * @param {string} endDate - 
     */
    GetDashboardHitsDaily(parameters: {
        'authorization' ? : string,
        'startDate' ? : string,
        'endDate' ? : string,
    } & CommonRequestOptions): Promise < ResponseWithBody < 200, Response_GetDashboardHitsDaily_200 > | ResponseWithBody < 401, void > | ResponseWithBody < 403, void > | ResponseWithBody < 500, void >> {
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/dashboard/hits/daily';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        let body: any;
        let queryParameters: QueryParameters = {};
        let headers: RequestHeaders = {};
        let form: any = {};
        return new Promise((resolve, reject) => {
            headers['Accept'] = 'text/plain, application/json, text/json';
            headers['Content-Type'] = '';

            if (parameters['authorization'] !== undefined) {
                headers['authorization'] = parameters['authorization'];
            }

            if (parameters['startDate'] !== undefined) {
                queryParameters['startDate'] = this.convertParameterCollectionFormat(
                    parameters['startDate'],
                    ''
                );
            }

            if (parameters['endDate'] !== undefined) {
                queryParameters['endDate'] = this.convertParameterCollectionFormat(
                    parameters['endDate'],
                    ''
                );
            }

            if (parameters.$queryParameters) {
                queryParameters = {
                    ...queryParameters,
                    ...parameters.$queryParameters
                };
            }

            this.request('GET', domain + path, body, headers, queryParameters, form, reject, resolve, parameters);
        });
    }

    GetDashboardInvoicesURL(parameters: {
        'authorization' ? : string,
    } & CommonRequestOptions): string {
        let queryParameters: QueryParameters = {};
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/dashboard/invoices';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        if (parameters.$queryParameters) {
            queryParameters = {
                ...queryParameters,
                ...parameters.$queryParameters
            };
        }

        let keys = Object.keys(queryParameters);
        return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '');
    }

    /**
     * 
     * @method
     * @name BackendEntity#GetDashboardInvoices
     * @param {string} authorization - 
     */
    GetDashboardInvoices(parameters: {
        'authorization' ? : string,
    } & CommonRequestOptions): Promise < ResponseWithBody < 200, Response_GetDashboardInvoices_200 > | ResponseWithBody < 401, void > | ResponseWithBody < 403, void > | ResponseWithBody < 500, void >> {
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/dashboard/invoices';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        let body: any;
        let queryParameters: QueryParameters = {};
        let headers: RequestHeaders = {};
        let form: any = {};
        return new Promise((resolve, reject) => {
            headers['Accept'] = 'text/plain, application/json, text/json';
            headers['Content-Type'] = '';

            if (parameters['authorization'] !== undefined) {
                headers['authorization'] = parameters['authorization'];
            }

            if (parameters.$queryParameters) {
                queryParameters = {
                    ...queryParameters,
                    ...parameters.$queryParameters
                };
            }

            this.request('GET', domain + path, body, headers, queryParameters, form, reject, resolve, parameters);
        });
    }

    GetApiKeysURL(parameters: {
        'authorization' ? : string,
    } & CommonRequestOptions): string {
        let queryParameters: QueryParameters = {};
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/plugin/api-keys';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        if (parameters.$queryParameters) {
            queryParameters = {
                ...queryParameters,
                ...parameters.$queryParameters
            };
        }

        let keys = Object.keys(queryParameters);
        return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '');
    }

    /**
     * 
     * @method
     * @name BackendEntity#GetApiKeys
     * @param {string} authorization - 
     */
    GetApiKeys(parameters: {
        'authorization' ? : string,
    } & CommonRequestOptions): Promise < ResponseWithBody < 200, Response_GetApiKeys_200 > | ResponseWithBody < 401, void > | ResponseWithBody < 403, void > | ResponseWithBody < 500, void >> {
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/plugin/api-keys';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        let body: any;
        let queryParameters: QueryParameters = {};
        let headers: RequestHeaders = {};
        let form: any = {};
        return new Promise((resolve, reject) => {
            headers['Accept'] = 'text/plain, application/json, text/json';
            headers['Content-Type'] = '';

            if (parameters['authorization'] !== undefined) {
                headers['authorization'] = parameters['authorization'];
            }

            if (parameters.$queryParameters) {
                queryParameters = {
                    ...queryParameters,
                    ...parameters.$queryParameters
                };
            }

            this.request('GET', domain + path, body, headers, queryParameters, form, reject, resolve, parameters);
        });
    }

    CreateApiKeyURL(parameters: {
        'authorization' ? : string,
    } & CommonRequestOptions): string {
        let queryParameters: QueryParameters = {};
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/plugin/api-keys';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        if (parameters.$queryParameters) {
            queryParameters = {
                ...queryParameters,
                ...parameters.$queryParameters
            };
        }

        queryParameters = {};

        let keys = Object.keys(queryParameters);
        return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '');
    }

    /**
     * 
     * @method
     * @name BackendEntity#CreateApiKey
     * @param {string} authorization - 
     */
    CreateApiKey(parameters: {
        'authorization' ? : string,
    } & CommonRequestOptions): Promise < ResponseWithBody < 200, ApiKeyEntity > | ResponseWithBody < 401, void > | ResponseWithBody < 403, void > | ResponseWithBody < 500, void >> {
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/plugin/api-keys';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        let body: any;
        let queryParameters: QueryParameters = {};
        let headers: RequestHeaders = {};
        let form: any = {};
        return new Promise((resolve, reject) => {
            headers['Accept'] = 'text/plain, application/json, text/json';
            headers['Content-Type'] = '';

            if (parameters['authorization'] !== undefined) {
                headers['authorization'] = parameters['authorization'];
            }

            if (parameters.$queryParameters) {
                queryParameters = {
                    ...queryParameters,
                    ...parameters.$queryParameters
                };
            }

            form = queryParameters;
            queryParameters = {};

            this.request('POST', domain + path, body, headers, queryParameters, form, reject, resolve, parameters);
        });
    }

    GetApiKeyURL(parameters: {
        'authorization' ? : string,
        'apiKeyId': string,
    } & CommonRequestOptions): string {
        let queryParameters: QueryParameters = {};
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/plugin/api-keys/{apiKeyId}';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        path = path.replace(
            '{apiKeyId}',
            `${encodeURIComponent(this.convertParameterCollectionFormat(
                        parameters['apiKeyId'],
                        ''
                    ).toString())}`
        );

        if (parameters.$queryParameters) {
            queryParameters = {
                ...queryParameters,
                ...parameters.$queryParameters
            };
        }

        let keys = Object.keys(queryParameters);
        return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '');
    }

    /**
     * 
     * @method
     * @name BackendEntity#GetApiKey
     * @param {string} authorization - 
     * @param {string} apiKeyId - 
     */
    GetApiKey(parameters: {
        'authorization' ? : string,
        'apiKeyId': string,
    } & CommonRequestOptions): Promise < ResponseWithBody < 200, ApiKeyEntity > | ResponseWithBody < 401, void > | ResponseWithBody < 403, void > | ResponseWithBody < 404, void > | ResponseWithBody < 500, void >> {
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/plugin/api-keys/{apiKeyId}';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        let body: any;
        let queryParameters: QueryParameters = {};
        let headers: RequestHeaders = {};
        let form: any = {};
        return new Promise((resolve, reject) => {
            headers['Accept'] = 'text/plain, application/json, text/json';
            headers['Content-Type'] = '';

            if (parameters['authorization'] !== undefined) {
                headers['authorization'] = parameters['authorization'];
            }

            path = path.replace(
                '{apiKeyId}',
                `${encodeURIComponent(this.convertParameterCollectionFormat(
                    parameters['apiKeyId'],
                    ''
                ).toString())}`
            );

            if (parameters['apiKeyId'] === undefined) {
                reject(new Error('Missing required  parameter: apiKeyId'));
                return;
            }

            if (parameters.$queryParameters) {
                queryParameters = {
                    ...queryParameters,
                    ...parameters.$queryParameters
                };
            }

            this.request('GET', domain + path, body, headers, queryParameters, form, reject, resolve, parameters);
        });
    }

    UpdateApiKeyURL(parameters: {
        'authorization' ? : string,
        'req' ? : PluginApiKeyRequest,
        'apiKeyId': string,
    } & CommonRequestOptions): string {
        let queryParameters: QueryParameters = {};
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/plugin/api-keys/{apiKeyId}';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        path = path.replace(
            '{apiKeyId}',
            `${encodeURIComponent(this.convertParameterCollectionFormat(
                        parameters['apiKeyId'],
                        ''
                    ).toString())}`
        );

        if (parameters.$queryParameters) {
            queryParameters = {
                ...queryParameters,
                ...parameters.$queryParameters
            };
        }

        let keys = Object.keys(queryParameters);
        return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '');
    }

    /**
     * 
     * @method
     * @name BackendEntity#UpdateApiKey
     * @param {string} authorization - 
     * @param {} req - 
     * @param {string} apiKeyId - 
     */
    UpdateApiKey(parameters: {
        'authorization' ? : string,
        'req' ? : PluginApiKeyRequest,
        'apiKeyId': string,
    } & CommonRequestOptions): Promise < ResponseWithBody < 200, ApiKeyEntity > | ResponseWithBody < 400, void > | ResponseWithBody < 401, void > | ResponseWithBody < 403, void > | ResponseWithBody < 404, void > | ResponseWithBody < 500, void >> {
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/plugin/api-keys/{apiKeyId}';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        let body: any;
        let queryParameters: QueryParameters = {};
        let headers: RequestHeaders = {};
        let form: any = {};
        return new Promise((resolve, reject) => {
            headers['Accept'] = 'text/plain, application/json, text/json';
            headers['Content-Type'] = 'application/json-patch+json';

            if (parameters['authorization'] !== undefined) {
                headers['authorization'] = parameters['authorization'];
            }

            if (parameters['req'] !== undefined) {
                body = parameters['req'];
            }

            path = path.replace(
                '{apiKeyId}',
                `${encodeURIComponent(this.convertParameterCollectionFormat(
                    parameters['apiKeyId'],
                    ''
                ).toString())}`
            );

            if (parameters['apiKeyId'] === undefined) {
                reject(new Error('Missing required  parameter: apiKeyId'));
                return;
            }

            if (parameters.$queryParameters) {
                queryParameters = {
                    ...queryParameters,
                    ...parameters.$queryParameters
                };
            }

            this.request('PUT', domain + path, body, headers, queryParameters, form, reject, resolve, parameters);
        });
    }

    DeleteApiKeyURL(parameters: {
        'authorization' ? : string,
        'apiKeyId': string,
    } & CommonRequestOptions): string {
        let queryParameters: QueryParameters = {};
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/plugin/api-keys/{apiKeyId}';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        path = path.replace(
            '{apiKeyId}',
            `${encodeURIComponent(this.convertParameterCollectionFormat(
                        parameters['apiKeyId'],
                        ''
                    ).toString())}`
        );

        if (parameters.$queryParameters) {
            queryParameters = {
                ...queryParameters,
                ...parameters.$queryParameters
            };
        }

        let keys = Object.keys(queryParameters);
        return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '');
    }

    /**
     * 
     * @method
     * @name BackendEntity#DeleteApiKey
     * @param {string} authorization - 
     * @param {string} apiKeyId - 
     */
    DeleteApiKey(parameters: {
        'authorization' ? : string,
        'apiKeyId': string,
    } & CommonRequestOptions): Promise < ResponseWithBody < 200, ActionResult > | ResponseWithBody < 401, void > | ResponseWithBody < 403, void > | ResponseWithBody < 404, void > | ResponseWithBody < 500, void >> {
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/plugin/api-keys/{apiKeyId}';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        let body: any;
        let queryParameters: QueryParameters = {};
        let headers: RequestHeaders = {};
        let form: any = {};
        return new Promise((resolve, reject) => {
            headers['Accept'] = 'text/plain, application/json, text/json';
            headers['Content-Type'] = '';

            if (parameters['authorization'] !== undefined) {
                headers['authorization'] = parameters['authorization'];
            }

            path = path.replace(
                '{apiKeyId}',
                `${encodeURIComponent(this.convertParameterCollectionFormat(
                    parameters['apiKeyId'],
                    ''
                ).toString())}`
            );

            if (parameters['apiKeyId'] === undefined) {
                reject(new Error('Missing required  parameter: apiKeyId'));
                return;
            }

            if (parameters.$queryParameters) {
                queryParameters = {
                    ...queryParameters,
                    ...parameters.$queryParameters
                };
            }

            this.request('DELETE', domain + path, body, headers, queryParameters, form, reject, resolve, parameters);
        });
    }

    DeleteApiKeyDomainURL(parameters: {
        'authorization' ? : string,
        'apiKeyId': string,
        'domainId': string,
    } & CommonRequestOptions): string {
        let queryParameters: QueryParameters = {};
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/plugin/api-keys/{apiKeyId}/domains/{domainId}';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        path = path.replace(
            '{apiKeyId}',
            `${encodeURIComponent(this.convertParameterCollectionFormat(
                        parameters['apiKeyId'],
                        ''
                    ).toString())}`
        );

        path = path.replace(
            '{domainId}',
            `${encodeURIComponent(this.convertParameterCollectionFormat(
                        parameters['domainId'],
                        ''
                    ).toString())}`
        );

        if (parameters.$queryParameters) {
            queryParameters = {
                ...queryParameters,
                ...parameters.$queryParameters
            };
        }

        let keys = Object.keys(queryParameters);
        return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '');
    }

    /**
     * 
     * @method
     * @name BackendEntity#DeleteApiKeyDomain
     * @param {string} authorization - 
     * @param {string} apiKeyId - 
     * @param {string} domainId - 
     */
    DeleteApiKeyDomain(parameters: {
        'authorization' ? : string,
        'apiKeyId': string,
        'domainId': string,
    } & CommonRequestOptions): Promise < ResponseWithBody < 200, ActionResult > | ResponseWithBody < 401, void > | ResponseWithBody < 403, void > | ResponseWithBody < 404, void > | ResponseWithBody < 500, void >> {
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/plugin/api-keys/{apiKeyId}/domains/{domainId}';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        let body: any;
        let queryParameters: QueryParameters = {};
        let headers: RequestHeaders = {};
        let form: any = {};
        return new Promise((resolve, reject) => {
            headers['Accept'] = 'text/plain, application/json, text/json';
            headers['Content-Type'] = '';

            if (parameters['authorization'] !== undefined) {
                headers['authorization'] = parameters['authorization'];
            }

            path = path.replace(
                '{apiKeyId}',
                `${encodeURIComponent(this.convertParameterCollectionFormat(
                    parameters['apiKeyId'],
                    ''
                ).toString())}`
            );

            if (parameters['apiKeyId'] === undefined) {
                reject(new Error('Missing required  parameter: apiKeyId'));
                return;
            }

            path = path.replace(
                '{domainId}',
                `${encodeURIComponent(this.convertParameterCollectionFormat(
                    parameters['domainId'],
                    ''
                ).toString())}`
            );

            if (parameters['domainId'] === undefined) {
                reject(new Error('Missing required  parameter: domainId'));
                return;
            }

            if (parameters.$queryParameters) {
                queryParameters = {
                    ...queryParameters,
                    ...parameters.$queryParameters
                };
            }

            this.request('DELETE', domain + path, body, headers, queryParameters, form, reject, resolve, parameters);
        });
    }

    CreateApiKeyDomainURL(parameters: {
        'authorization' ? : string,
        'apiKeyId': string,
        'req' ? : PluginApiKeyDomainRequest,
    } & CommonRequestOptions): string {
        let queryParameters: QueryParameters = {};
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/plugin/api-keys/{apiKeyid}/domains';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        path = path.replace(
            '{apiKeyId}',
            `${encodeURIComponent(this.convertParameterCollectionFormat(
                        parameters['apiKeyId'],
                        ''
                    ).toString())}`
        );

        if (parameters.$queryParameters) {
            queryParameters = {
                ...queryParameters,
                ...parameters.$queryParameters
            };
        }

        queryParameters = {};

        let keys = Object.keys(queryParameters);
        return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '');
    }

    /**
     * 
     * @method
     * @name BackendEntity#CreateApiKeyDomain
     * @param {string} authorization - 
     * @param {string} apiKeyId - 
     * @param {} req - 
     */
    CreateApiKeyDomain(parameters: {
        'authorization' ? : string,
        'apiKeyId': string,
        'req' ? : PluginApiKeyDomainRequest,
    } & CommonRequestOptions): Promise < ResponseWithBody < 200, DomainEntity > | ResponseWithBody < 401, void > | ResponseWithBody < 403, void > | ResponseWithBody < 404, void > | ResponseWithBody < 500, void >> {
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/plugin/api-keys/{apiKeyid}/domains';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        let body: any;
        let queryParameters: QueryParameters = {};
        let headers: RequestHeaders = {};
        let form: any = {};
        return new Promise((resolve, reject) => {
            headers['Accept'] = 'text/plain, application/json, text/json';
            headers['Content-Type'] = 'application/json-patch+json';

            if (parameters['authorization'] !== undefined) {
                headers['authorization'] = parameters['authorization'];
            }

            path = path.replace(
                '{apiKeyId}',
                `${encodeURIComponent(this.convertParameterCollectionFormat(
                    parameters['apiKeyId'],
                    ''
                ).toString())}`
            );

            if (parameters['apiKeyId'] === undefined) {
                reject(new Error('Missing required  parameter: apiKeyId'));
                return;
            }

            if (parameters['req'] !== undefined) {
                body = parameters['req'];
            }

            if (parameters.$queryParameters) {
                queryParameters = {
                    ...queryParameters,
                    ...parameters.$queryParameters
                };
            }

            form = queryParameters;
            queryParameters = {};

            this.request('POST', domain + path, body, headers, queryParameters, form, reject, resolve, parameters);
        });
    }

    GetConfigURL(parameters: {
        'authorization' ? : string,
    } & CommonRequestOptions): string {
        let queryParameters: QueryParameters = {};
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/plugin/config';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        if (parameters.$queryParameters) {
            queryParameters = {
                ...queryParameters,
                ...parameters.$queryParameters
            };
        }

        let keys = Object.keys(queryParameters);
        return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '');
    }

    /**
     * 
     * @method
     * @name BackendEntity#GetConfig
     * @param {string} authorization - 
     */
    GetConfig(parameters: {
        'authorization' ? : string,
    } & CommonRequestOptions): Promise < ResponseWithBody < 200, ConfigEntity > | ResponseWithBody < 401, void > | ResponseWithBody < 403, void > | ResponseWithBody < 404, void > | ResponseWithBody < 500, void >> {
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/plugin/config';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        let body: any;
        let queryParameters: QueryParameters = {};
        let headers: RequestHeaders = {};
        let form: any = {};
        return new Promise((resolve, reject) => {
            headers['Accept'] = 'text/plain, application/json, text/json';
            headers['Content-Type'] = '';

            if (parameters['authorization'] !== undefined) {
                headers['authorization'] = parameters['authorization'];
            }

            if (parameters.$queryParameters) {
                queryParameters = {
                    ...queryParameters,
                    ...parameters.$queryParameters
                };
            }

            this.request('GET', domain + path, body, headers, queryParameters, form, reject, resolve, parameters);
        });
    }

    UpdateConfigURL(parameters: {
        'authorization' ? : string,
        'request' ? : PluginConfigRequest,
    } & CommonRequestOptions): string {
        let queryParameters: QueryParameters = {};
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/plugin/config';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        if (parameters.$queryParameters) {
            queryParameters = {
                ...queryParameters,
                ...parameters.$queryParameters
            };
        }

        let keys = Object.keys(queryParameters);
        return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '');
    }

    /**
     * 
     * @method
     * @name BackendEntity#UpdateConfig
     * @param {string} authorization - 
     * @param {} request - 
     */
    UpdateConfig(parameters: {
        'authorization' ? : string,
        'request' ? : PluginConfigRequest,
    } & CommonRequestOptions): Promise < ResponseWithBody < 200, ConfigEntity > | ResponseWithBody < 401, void > | ResponseWithBody < 403, void > | ResponseWithBody < 404, void > | ResponseWithBody < 500, void >> {
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/plugin/config';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        let body: any;
        let queryParameters: QueryParameters = {};
        let headers: RequestHeaders = {};
        let form: any = {};
        return new Promise((resolve, reject) => {
            headers['Accept'] = 'text/plain, application/json, text/json';
            headers['Content-Type'] = 'application/json-patch+json';

            if (parameters['authorization'] !== undefined) {
                headers['authorization'] = parameters['authorization'];
            }

            if (parameters['request'] !== undefined) {
                body = parameters['request'];
            }

            if (parameters.$queryParameters) {
                queryParameters = {
                    ...queryParameters,
                    ...parameters.$queryParameters
                };
            }

            this.request('PUT', domain + path, body, headers, queryParameters, form, reject, resolve, parameters);
        });
    }

    GetTimeZonesURL(parameters: {} & CommonRequestOptions): string {
        let queryParameters: QueryParameters = {};
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/timezones';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        if (parameters.$queryParameters) {
            queryParameters = {
                ...queryParameters,
                ...parameters.$queryParameters
            };
        }

        let keys = Object.keys(queryParameters);
        return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '');
    }

    /**
     * 
     * @method
     * @name BackendEntity#GetTimeZones
     */
    GetTimeZones(parameters: {} & CommonRequestOptions): Promise < ResponseWithBody < 200, Response_GetTimeZones_200 > | ResponseWithBody < 401, void > | ResponseWithBody < 403, void > | ResponseWithBody < 500, void >> {
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/timezones';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        let body: any;
        let queryParameters: QueryParameters = {};
        let headers: RequestHeaders = {};
        let form: any = {};
        return new Promise((resolve, reject) => {
            headers['Accept'] = 'text/plain, application/json, text/json';
            headers['Content-Type'] = '';

            if (parameters.$queryParameters) {
                queryParameters = {
                    ...queryParameters,
                    ...parameters.$queryParameters
                };
            }

            this.request('GET', domain + path, body, headers, queryParameters, form, reject, resolve, parameters);
        });
    }

    GetTimeZonesByCountryURL(parameters: {
        'country': string,
    } & CommonRequestOptions): string {
        let queryParameters: QueryParameters = {};
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/timezones/{country}';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        path = path.replace(
            '{country}',
            `${encodeURIComponent(this.convertParameterCollectionFormat(
                        parameters['country'],
                        ''
                    ).toString())}`
        );

        if (parameters.$queryParameters) {
            queryParameters = {
                ...queryParameters,
                ...parameters.$queryParameters
            };
        }

        let keys = Object.keys(queryParameters);
        return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '');
    }

    /**
     * 
     * @method
     * @name BackendEntity#GetTimeZonesByCountry
     * @param {string} country - 
     */
    GetTimeZonesByCountry(parameters: {
        'country': string,
    } & CommonRequestOptions): Promise < ResponseWithBody < 200, Response_GetTimeZonesByCountry_200 > | ResponseWithBody < 401, void > | ResponseWithBody < 403, void > | ResponseWithBody < 500, void >> {
        const domain = parameters.$domain ? parameters.$domain : this.domain;
        let path = '/timezones/{country}';
        if (parameters.$path) {
            path = (typeof(parameters.$path) === 'function') ? parameters.$path(path) : parameters.$path;
        }

        let body: any;
        let queryParameters: QueryParameters = {};
        let headers: RequestHeaders = {};
        let form: any = {};
        return new Promise((resolve, reject) => {
            headers['Accept'] = 'text/plain, application/json, text/json';
            headers['Content-Type'] = '';

            path = path.replace(
                '{country}',
                `${encodeURIComponent(this.convertParameterCollectionFormat(
                    parameters['country'],
                    ''
                ).toString())}`
            );

            if (parameters['country'] === undefined) {
                reject(new Error('Missing required  parameter: country'));
                return;
            }

            if (parameters.$queryParameters) {
                queryParameters = {
                    ...queryParameters,
                    ...parameters.$queryParameters
                };
            }

            this.request('GET', domain + path, body, headers, queryParameters, form, reject, resolve, parameters);
        });
    }

}

export default BackendEntity;