import backend from '../../../config/backend.json'
import { BaseController } from '../../BaseController'

import { ConfigEntity } from '../../../entities/api'
import PluginConfig from './PluginConfig'

export class PluginConfigController extends BaseController {
    private component?: PluginConfig = undefined

    constructor(component: PluginConfig) {
        super()
        this.component = component
    }

    initialize = () => {
        this.initializeConfig()
    }

    initializeConfig = () => {
        this.component!.setState({ configLoading: true })
        this.getConfigBackend(
            (config: ConfigEntity) => {
                this.component!.setState({ config, configLoading: false, isLoaded: true })
            },
            (e: any) => {
                this.component!.setState({ configLoading: false, configError: true, isLoaded: true })
            }
        )
    }

    handleChange = (config: string) => {
        const callback = () => {
            this.saveConfigBackend(
                this.component!.state.config,
                (json: any) => {},
                (e: any) => {
                    this.component!.setState({ configLoading: false, configError: true })
                }
            )
        }

        switch (config) {
            case 'embedExternalImages':
                this.component!.setState({ ...this.component!.state, config: { ...this.component!.state.config, embedExternalImages: !this.component!.state.config.embedExternalImages } }, callback)
                break
            case 'removeEmptyTags':
                this.component!.setState({ ...this.component!.state, config: { ...this.component!.state.config, removeEmptyTags: !this.component!.state.config.removeEmptyTags } }, callback)
                break
            case 'removeSpanTags':
                this.component!.setState({ ...this.component!.state, config: { ...this.component!.state.config, removeSpanTags: !this.component!.state.config.removeSpanTags } }, callback)
                break
            case 'removeClassNames':
                this.component!.setState({ ...this.component!.state, config: { ...this.component!.state.config, removeClassNames: !this.component!.state.config.removeClassNames } }, callback)
                break
            case 'removeIframes':
                this.component!.setState({ ...this.component!.state, config: { ...this.component!.state.config, removeIframes: !this.component!.state.config.removeIframes } }, callback)
                break
            case 'removeTagAttributes':
                this.component!.setState({ ...this.component!.state, config: { ...this.component!.state.config, removeTagAttributes: !this.component!.state.config.removeTagAttributes } }, callback)
                break
        }
    }

    private getConfigBackend = (success?: (json: any) => void, error?: (e: any) => void) => {
        this.callBackend(this.endpoint(`/config`), 'GET', success, error)
    }

    private saveConfigBackend = (config: ConfigEntity, success?: (json: any) => void, error?: (e: any) => void) => {
        this.callBackendWithPayload(this.endpoint(`/config`), 'PUT', { config }, success, error)
    }

    private endpoint(path: string) {
        return this.culturedEndpoint(backend.endpoints.plugin + path)
    }
}
